using Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EFRepository
{
    public class Repository : IRepository , IDisposable
    {
        //Contexto
        protected DbContext context;

        //Contrusctor
        //A pesar que DbContext es un repositorio quiero que se inyecte cualquier fuente de datos
        //Creamos un repositorio que trabaje con cualquier contexto
        //Inyectamos esa dependencia. Un constructor en el cual pueda inyectar el contexto

        //Si yo solo quiero consultar y no realizar cambios en mis entidades lo puedo optimizar para que el ORM
        //no vigile esos cambios que nunca se van a presentar, pero se lo pido todo al usuario.
        public Repository (DbContext context, 
                             bool autoDetectChangesEnabled=false,
                             bool proxyCreationEnabled=false)
        {
            this.context = context;
            this.context.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
            this.context.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
        }


        TEntity IRepository.Create<TEntity>(TEntity newEntity)
        {
            TEntity result = null;
            try
            {
                result = context.Set<TEntity>().Add(newEntity);
                //Llamamos a un mètodo que hemos creado 
                TrySaveChanges();
            }
            catch(Exception e)
            {
                throw e;
            }
            return result;
        }

        //Metodo que pueda ser sobreescrito para utilizarlo en UnitOfWork
        protected virtual int TrySaveChanges()
        {
            return context.SaveChanges();
        }
        public bool Delete<TEntity>(TEntity delitedEntity) where TEntity : class
        {
            bool result = false;
            try
            {
                context.Set<TEntity>().Attach(delitedEntity);
                context.Set<TEntity>().Remove(delitedEntity);
                result = TrySaveChanges() > 0;
            }
            catch(Exception e)
            {
                throw e;
            }
           return result;

        }

        public void Dispose()
        {
          if (context != null)
            {
                context.Dispose();
            }
               
        }

        public TEntity FindEntity<TEntity>(Expression<Func<TEntity, bool>> criterio) where TEntity : class
        {
            TEntity result = null;
            try
            {
                result = context.Set<TEntity>().FirstOrDefault(criterio);
            }
            catch(Exception e)
            {
                throw e;
            }
            return result;
        }

        public IEnumerable<TEntity> FindEntitySet<TEntity>(Expression<Func<TEntity, bool>> criterio) where TEntity : class
        {
            List<TEntity> result = null;
            try
            {
                result = context.Set<TEntity>().Where(criterio).ToList();
            }
            catch(Exception e)
            {
                throw e;
            }

            return result;
        }

        public bool Update<TEntity>(TEntity modifiedEntity) where TEntity : class
        {
            bool result = false;
            try
            {
                context.Set<TEntity>().Attach(modifiedEntity);
                context.Entry<TEntity>(modifiedEntity).State = EntityState.Modified;
                //Si es verdadero sera true
                result = TrySaveChanges() > 0;
            }
            catch(Exception e)
            {
                throw e;
            }
            return result;
        }
    }
}
