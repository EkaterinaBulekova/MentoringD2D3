using Contracts;
using Entities;
using FoodShopDAL;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _repoContext;
        private IProductRepository _product;
        private ICategorytRepository _category;
        private ICartRepository _cart;

        public IProductRepository Product
        {
            get
            {
                if (_product == null)
                {
                    _product = new ProductRepository(_repoContext);
                }

                return _product;
            }
        }

        public ICategorytRepository Category
        {
            get
            {
                if (_category == null)
                {
                    _category = new CategoryRepository(_repoContext);
                }

                return _category;
            }
        }

        public ICartRepository Cart
        {
            get
            {
                if (_cart == null)
                {
                    _cart = new CartRepository(_repoContext);
                }

                return _cart;
            }
        }

        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }
    }
}
