using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using MauiAppCrud.DataAccess;
using MauiAppCrud.DTO;
using Microsoft.EntityFrameworkCore;
using MauiAppCrud.Utilities;
using System.Collections.ObjectModel;
using MauiAppCrud.Views;

namespace MauiAppCrud.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly EmpleadoDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<EmpleadoDTO> listaEmpleado = new ObservableCollection<EmpleadoDTO> ();

        public MainViewModel(EmpleadoDbContext context)
        {
            _dbContext = context;
            MainThread.BeginInvokeOnMainThread(new Action(async () => await Obtener()));

            WeakReferenceMessenger.Default.Register<EmpleadoMensajeria>(this, (r, m) => {
                EmpleadoMensajeRecibido(m.Value);
            });
        }

        public async Task Obtener()
        {
            var lista = await _dbContext.EmpleadoSet.ToListAsync();

            // Validar si hay registros
            if (lista.Any())
            {
                foreach (var empleado in lista)
                {
                    ListaEmpleado.Add(new EmpleadoDTO
                    {
                        Id = empleado.Id,
                        Nombre = empleado.Nombre,
                        Email = empleado.Email,
                        Sueldo = empleado.Sueldo,
                        FechaContrato = empleado.FechaContrato,
                    });
                }
            }
        }

        private void EmpleadoMensajeRecibido(EmpleadoMensaje empleadoMensaje)
        {
            var empleadoDto = empleadoMensaje.EmpleadoDTO;

            if (empleadoMensaje.crear)
            {
                ListaEmpleado.Add(empleadoDto);
            } else
            {
                var encontrado = ListaEmpleado.First(e => e.Id == empleadoDto.Id);

                encontrado.Nombre = empleadoDto.Nombre;
                encontrado.Email = empleadoDto.Email;
                encontrado.Sueldo = empleadoDto.Sueldo;
                encontrado.FechaContrato = empleadoDto.FechaContrato;
            }
        }

        [RelayCommand]
        private async Task Crear()
        {
            var uri = $"{nameof(EmpleadoPage)}?id=0";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Editar(EmpleadoDTO empleadoDto)
        {
            var uri = $"{nameof(EmpleadoPage)}?id={empleadoDto.Id}";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Eliminar(EmpleadoDTO empleadoDto)
        {
            bool respuesta = await Shell.Current.DisplayAlert("Eliminar Empleado", "¿Desea eliminar el empleado?", "Si", "No");

            if (respuesta)
            {
                var encontrado = await _dbContext.EmpleadoSet.FirstAsync(e => e.Id == empleadoDto.Id);

                //Eliminar desde DB
                _dbContext.EmpleadoSet.Remove(encontrado);
                await _dbContext.SaveChangesAsync();

                //Eliminar de la lista
                ListaEmpleado.Remove(empleadoDto);
            }
        }
    }
}
