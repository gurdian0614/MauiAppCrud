using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using MauiAppCrud.DataAccess;
using MauiAppCrud.DTO;
using Microsoft.EntityFrameworkCore;
using MauiAppCrud.Utilities;
using MauiAppCrud.Models;

namespace MauiAppCrud.ViewModels
{
    public partial class EmpleadoViewModel : ObservableObject, IQueryAttributable
    {
        private readonly EmpleadoDbContext _dbContext;

        [ObservableProperty]
        private EmpleadoDTO empleadoDto = new EmpleadoDTO();

        [ObservableProperty]
        private string titulo;

        private int Id;

        [ObservableProperty]
        private bool loadingEsVisible = false;

        public EmpleadoViewModel(EmpleadoDbContext context)
        {
            _dbContext = context;
            EmpleadoDto.FechaContrato = DateTime.Now;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Id = int.Parse(query["id"].ToString());

            //Titulo
            if (Id == 0)
            {
                Titulo = "Nuevo Empleado";
            } else
            {
                Titulo = "Editar Empleado";
                LoadingEsVisible = true;

                //Cargar la data para mostrar en el formulario
                await Task.Run(async () => {
                    var encontrado = await _dbContext.EmpleadoSet.FirstAsync(e => e.Id == Id);
                    EmpleadoDto.Id = encontrado.Id;
                    EmpleadoDto.Nombre = encontrado.Nombre;
                    EmpleadoDto.Email = encontrado.Email;
                    EmpleadoDto.Sueldo = encontrado.Sueldo;
                    EmpleadoDto.FechaContrato = encontrado.FechaContrato;

                    //Regresar al hilo principal
                    MainThread.BeginInvokeOnMainThread(() => LoadingEsVisible = false);
                });
            }
        }

        [RelayCommand]
        private async Task Guardar()
        {
            LoadingEsVisible = true;
            EmpleadoMensaje mensaje = new EmpleadoMensaje();

            //Crear tarea en segundo plano
            // Guardar o editar empleado
            await Task.Run(async () => {
                if (Id == 0)
                {
                    var tbEmpleado = new Empleado
                    {
                        Nombre = EmpleadoDto.Nombre, 
                        Email = EmpleadoDto.Email,
                        Sueldo = EmpleadoDto.Sueldo,
                        FechaContrato = EmpleadoDto.FechaContrato,
                    };

                    //Proceso para guardar registro a la DB
                    _dbContext.EmpleadoSet.Add(tbEmpleado);
                    await _dbContext.SaveChangesAsync();

                    EmpleadoDto.Id = tbEmpleado.Id;
                    mensaje = new EmpleadoMensaje()
                    {
                        crear = true,
                        EmpleadoDTO = EmpleadoDto
                    };
                } else
                {
                    var encontrado = await _dbContext.EmpleadoSet.FirstAsync(e => e.Id == Id);
                    encontrado.Nombre = EmpleadoDto.Nombre;
                    encontrado.Email = EmpleadoDto.Email;
                    encontrado.Sueldo = EmpleadoDto.Sueldo;
                    encontrado.FechaContrato = EmpleadoDto.FechaContrato;

                    await _dbContext.SaveChangesAsync();
                    mensaje = new EmpleadoMensaje()
                    {
                        crear = false,
                        EmpleadoDTO = EmpleadoDto
                    };
                }

                //Regresar al hilo principal
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    LoadingEsVisible = false;
                    WeakReferenceMessenger.Default.Send(new EmpleadoMensajeria(mensaje));

                    //Regresar al main
                    await Shell.Current.Navigation.PopAsync();
                });

            });
        }
    }
}
