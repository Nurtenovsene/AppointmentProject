using Appointment.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;


namespace Appointment.DataAccess.Repositories
{
    public interface IRepository<T> where T : BaseEntity, new()
    {
        /// <summary>
        /// Veritabanına asenkron olarak yeni bir entity ekler.
        /// </summary>
        /// <param name="entity">Eklemek istediğiniz entity.</param>
        Task AddAsync(T entity);

        Task AddListAsync(List<T> entity);


        /// <summary>
        /// Belirtilen koşulu sağlayan herhangi bir entity'nin mevcut olup olmadığını asenkron olarak belirler.
        /// </summary>
        /// <param name="predicate">Her bir entity'nin bir koşul için test eden bir işlev.</param>
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Belirtilen koşula uyan entity'lerin sayısını asenkron olarak hesaplar.
        /// </summary>
        /// <param name="predicate">Her bir entity'i bir koşul için test eden bir işlev. Eğer null ise, tüm entity'leri sayar.</param>
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);

        /// <summary>
        /// Belirtilen koşula uyan bir liste entity'i asenkron olarak getirir. Opsiyonel olarak ilişkili entity'leri, sıralama ve sayfalama da ekleyebilirsiniz.
        /// </summary>
        /// <param name="predicate">Her bir varlığı bir koşul için test eden bir işlev. Eğer null ise, tüm entityleri getirir.</param>
        /// <param name="include">Sorguya dahil edilecek ilişkili entity'leri belirtmek için kullanılan bir işlev.</param>
        /// <param name="orderBy">Entity'lerin sırasını belirtmek için kullanılan bir işlev.</param>
        /// <param name="enableTracking">Entity tracking (Okuma işlemleri için: false, Yazma işlemleri için: true)</param>
        /// <param name="currentPage">Sayfalama için geçerli sayfa numarası. Varsayılan değer 1.</param>
        /// <param name="pageSize">Her sayfada getirilecek varlık sayısı. Varsayılan değer 3.</param>
        Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool enableTracking = true,
            int currentPage = 1,
            int pageSize = 100
        );

        Task<List<TRes>> GetAllAsync<TRes>(
            Expression<Func<T, TRes>> select,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool enableTracking = true,
            int currentPage = 1,
            int pageSize = 100
        );


        Task<List<T>> GetAllNoPaginationAsync(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool enableTracking = true
        );

        Task<List<TRes>> GetAllNoPaginationAsync<TRes>(
            Expression<Func<T, TRes>> select,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool enableTracking = true
        );

        /// <summary>
        /// Belirtilen koşulu sağlayan tek bir entity'i asenkron olarak getirir.
        /// </summary>
        /// <param name="predicate">Her bir entity'i bir koşul için test eden bir işlev. Koşulu sağlayan tek bir entity döner.</param>
        /// <param name="include">Sorguya dahil edilecek ilişkili entity'leri belirtmek için kullanılan bir işlev.</param>
        /// <param name="enableTracking">Entity tracking (Okuma işlemleri için: false, Yazma işlemleri için: true)</param>
        Task<T?> GetAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool enableTracking = true
        );

        Task<TRes> GetAsync<TRes>(
            Expression<Func<T, TRes>> select,
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool enableTracking = true
        );


    }
}
