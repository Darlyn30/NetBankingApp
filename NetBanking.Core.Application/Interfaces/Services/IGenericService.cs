

namespace NetBanking.Core.Application.Interfaces.Services
{
    public interface IGenericService<SaveViewModel, ViewModel, Entity>
           where SaveViewModel : class
           where ViewModel : class
           where Entity : class
    {
        Task Update(SaveViewModel vm, int id);
        Task UpdateProduct(SaveViewModel vm, string id);
        Task<SaveViewModel> Add(SaveViewModel vm);
        Task Delete(int id);
        Task<SaveViewModel> GetByIdSaveViewModel(int id);
        Task<List<ViewModel>> GetAllViewModel();
    }
}
