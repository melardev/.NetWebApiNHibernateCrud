using NHibernate;

namespace WebApiNHibernateCrud.Data
{
    public interface ISessionFactoryBuilder
    {
        ISessionFactory GetSessionFactory();
    }
}