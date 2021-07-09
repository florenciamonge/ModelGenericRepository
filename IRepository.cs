using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository
{
    public interface IRepository : IDisposable
    { 
        //Agrega una nueva entidad y el dato que recibe es de ese mismo tipo
        //Devuelve el mismo tipo por si lo necesito 
        TEntity Create<TEntity>(TEntity newEntity) where TEntity : class;

        //Modifica una entidad del tipo ingresado y devuelve un bool 
        bool Update<TEntity>(TEntity modifiedEntity) where TEntity : class;

        //Elimina una entidad del tipo ingresado y devuelve un bool 
        bool Delete<TEntity>(TEntity delitedEntity) where TEntity : class;

        //Encuentra una entidad del tipo especificado y devuelve del mismo tipo en base a un criterio
        //La expresion sera lambda y pertenece a System.Linq.Expressions;
        //La expresion recibe un tipo. La condicion debe venir de un mètodo que se le pueda proporcionar la entidad y 
        //que me debuelva bool.La expresion va a esperar un delegado. 
        TEntity FindEntity<TEntity>(Expression<Func<TEntity, bool>> criterio) where TEntity : class;

        //Devuelve una coleccion de entidades, un conjunto del tipo que especifico
        //Va a contar con un criterio al devolverlas
        IEnumerable<TEntity>FindEntitySet<TEntity>(Expression<Func<TEntity, bool>> criterio) where TEntity : class;
    }


}
