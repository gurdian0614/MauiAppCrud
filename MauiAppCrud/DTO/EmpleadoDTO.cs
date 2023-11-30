using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiAppCrud.DTO
{
    public partial class EmpleadoDTO : ObservableObject
    {
        [ObservableProperty]
        public int id;
        [ObservableProperty]
        public string nombre; 
        [ObservableProperty] 
        public string email;
        [ObservableProperty]
        public decimal sueldo;
        [ObservableProperty]
        public DateTime fechaContrato;
    }
}
