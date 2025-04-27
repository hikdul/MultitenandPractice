using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using multiTenandTest.entitys;
using multiTenandTest.Helpers;
using multiTenandTest.Services;

namespace multiTenandTest.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private string tenantId;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IServicioTenant servicioTenant
        )
            : base(options)
        {
            tenantId = servicioTenant.ObtenerTenant();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (
                var item in ChangeTracker
                    .Entries()
                    .Where(e => e.State == EntityState.Added && e.Entity is ITenandEntity)
            )
            {
                if (string.IsNullOrEmpty(tenantId))
                {
                    throw new Exception("TenandId no encontrado al momento de crear el registro.");
                }

                var entidad = item.Entity as ITenandEntity;
                entidad!.TenantId = tenantId;
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //?: para que las migraciones no me generen resultados en cascada al borrar elementos
            ////foreach (
            ////    var foreignKey in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())
            ////)
            ////{
            ////    foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
            ////}

            builder
                .Entity<EmpresaUsuarioPermisos>()
                .HasKey(x => new
                {
                    x.EmpresaId,
                    x.UserId,
                    x.permiso
                });

            builder
                .Entity<Pais>()
                .HasData(
                    new Pais[]
                    {
                        new Pais { Id = Guid.NewGuid().ToString(), Name = "República Dominicana" },
                        new Pais { Id = Guid.NewGuid().ToString(), Name = "México" },
                        new Pais { Id = Guid.NewGuid().ToString(), Name = "Colombia" },
                        new Pais { Id = Guid.NewGuid().ToString(), Name = "Venezuela" }
                    }
                );

            foreach (var entidad in builder.Model.GetEntityTypes())
            {
                var tipo = entidad.ClrType;

                if (typeof(ITenandEntity).IsAssignableFrom(tipo))
                {
                    var método = typeof(ApplicationDbContext)
                        .GetMethod(
                            nameof(ArmarFiltroGlobalTenant),
                            BindingFlags.NonPublic | BindingFlags.Static
                        )
                        ?.MakeGenericMethod(tipo);

                    var filtro = método?.Invoke(null, new object[] { this })!;
                    entidad.SetQueryFilter((LambdaExpression)filtro);
                    entidad.AddIndex(entidad.FindProperty(nameof(ITenandEntity.TenantId))!);
                }
                else if (tipo.DebeSaltarValidaciónTenant())
                {
                    continue;
                }
                else
                {
                    throw new Exception(
                        $"La entidad {entidad} no ha sido marcada como tenant o común"
                    );
                }
            }
        }

        private static LambdaExpression ArmarFiltroGlobalTenant<TEntidad>(
            ApplicationDbContext context
        )
            where TEntidad : class, ITenandEntity
        {
            Expression<Func<TEntidad, bool>> filtro = x => x.TenantId == context.tenantId;
            return filtro;
        }

        #region logica de negocios

        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<Pais> Paises => Set<Pais>();
        public DbSet<Empresa> Empresa => Set<Empresa>();
        public DbSet<EmpresaUsuarioPermisos> EmpresaUsuarioPermisos =>
            Set<EmpresaUsuarioPermisos>();
        public DbSet<Vinculacion> Vinculacion => Set<Vinculacion>();

        #endregion
    }
}
