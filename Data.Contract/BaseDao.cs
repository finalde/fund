using System;

namespace Data.Contract
{
    public interface IBaseDao<TKey>
    {
        public TKey Id { get; set; }
    }
    public class BaseDao
    {

    }
    public class BaseDao<TKey>: BaseDao, IBaseDao<TKey>
    {
        public TKey Id { get; set; }
    }
    public class BaseAuditableDao
    {
        public bool Active { get; set; }
        public string AuditBy { get; set; }
        public string EventType { get; set; }
        public DateTime EventTime { get; set; }
        public DateTime? DbTime { get; set; }
    }
    public class BaseAuditableDao<TKey>: BaseAuditableDao, IBaseDao<TKey>
    {
        public TKey Id { get; set; }
    }
    public class BaseTypeDao: BaseAuditableDao
    {
        public string Description { get; set; }
    }
    public class BaseTypeDao<TKey> : BaseTypeDao, IBaseDao<TKey>
    {
        public TKey Id { get; set; }
    }
    public class BaseDateDao:BaseAuditableDao
    {
        public DateTime Date { get; set; }
    }
    public class BaseDateDao<TKey> : BaseDateDao, IBaseDao<TKey>
    {
        public TKey Id { get; set; }
    }
}
