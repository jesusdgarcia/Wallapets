
using BL;
using ENTITIES;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallapets.Model.Utilities;

namespace Wallapets.Model
{
    public class clsDashboardVM : clsBase
    {
        #region Atributos
        private ObservableCollection<clsMascota> listadoMascotas;
        private clsMascota mascotaSeleccionada;
        private DelegateCommand actualizarListadoCommand;
        private IList<clsProvincia> provincias;
        private IList<clsTipo> tipos;
        private clsProvincia provinciaSeleccionada;
        private clsTipo tipoSeleccionada;
        private DelegateCommand buscarCommand;
        private DelegateCommand cargarMasItemsCommand;
        private bool isRefreshing;
        #endregion

        #region Propiedades
        public ObservableCollection<clsMascota> ListadoMascotas
        {
            get { return listadoMascotas; }
            set
            {
                listadoMascotas = value;
                NotifyPropertyChanged(nameof(ListadoMascotas));
            }
        }

        public clsMascota MascotaSeleccionada
        {
            get { return mascotaSeleccionada; }
            set
            {
                if (mascotaSeleccionada != value)
                {
                    mascotaSeleccionada = value;
                    DetalleMascota();
                }
            }
        }

        public DelegateCommand ActualizarListadoCommand
        {
            get
            {
                return actualizarListadoCommand;
            }
        }

        public DelegateCommand CargarMasItemsCommand
        {
            get
            {
                return cargarMasItemsCommand;
            }
        }

        public IList<clsProvincia> Provincias
        {
            get { return provincias; }
            set
            {
                provincias = value;
                NotifyPropertyChanged(nameof(Provincias));
            }
        }

        public IList<clsTipo> Tipos
        {
            get { return tipos; }
            set
            {
                tipos = value;
                NotifyPropertyChanged(nameof(Tipos));
            }
        }

        public clsProvincia ProvinciaSelecionada
        {
            get { return provinciaSeleccionada; }
            set
            {
                provinciaSeleccionada = value;
            }
        }

        public clsTipo TipoSelecionada
        {
            get { return tipoSeleccionada; }
            set
            {
                tipoSeleccionada = value;
            }
        }

        public DelegateCommand BuscarCommand
        {
            get => buscarCommand;
        }


        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }
            set
            {
                isRefreshing = value;
                NotifyPropertyChanged(nameof(IsRefreshing));
            }
        }
        #endregion

        public clsDashboardVM()
        {
            CargarMascotas();
            CargarLista();
            actualizarListadoCommand = new DelegateCommand(ActualizarListadoCommand_execute);
            buscarCommand = new DelegateCommand(BuscarCommand_execute);
            cargarMasItemsCommand = new DelegateCommand(CargarMasItemsCommand_execute);
        }

        #region Metodos
        /// <summary>
        /// Metodo que envia una mascota a una vista mas detallada del mismo
        /// </summary>
        private async void DetalleMascota()
        {
            var navParameter = new Dictionary<string, object>
            {
                { "mascota", mascotaSeleccionada}
            };
            await Shell.Current.GoToAsync("detallespage", navParameter);
        }

        /// <summary>
        /// Metodo que carga un listado de mascotas traido de la base de datos
        /// </summary>
        public async void CargarMascotas()
        {
            try
            {
                listadoMascotas = new ObservableCollection<clsMascota>(await clsFirebaseRealtimeBL.GetPetsBL(null, Preferences.Get("UserToken", "")));
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Error al cargar el listado de mascotas", "Recargar");
                CargarMascotas();
            }
            NotifyPropertyChanged(nameof(ListadoMascotas));
        }

        /// <summary>
        /// Metodo que busca segun la provincia, la especie o ambos
        /// </summary>
        public async void BuscarCommand_execute()
        {
            try
            {
                if (tipoSeleccionada != null && provinciaSeleccionada != null)
                {
                    listadoMascotas = new ObservableCollection<clsMascota>(await clsFirebaseRealtimeBL.GetPetsProvinciaTipoBL(provinciaSeleccionada.Name, tipoSeleccionada.Name, Preferences.Get("UserToken", "")));
                }
                else if (tipoSeleccionada != null)
                {
                    listadoMascotas = new ObservableCollection<clsMascota>(await clsFirebaseRealtimeBL.GetPetsTipoBL(tipoSeleccionada.Name, Preferences.Get("UserToken", "")));
                }
                else if (provinciaSeleccionada != null)
                {
                    listadoMascotas = new ObservableCollection<clsMascota>(await clsFirebaseRealtimeBL.GetPetsProvinciaBL(provinciaSeleccionada.Name, Preferences.Get("UserToken", "")));
                }
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Error al buscar", "Ok");
            }
            NotifyPropertyChanged(nameof(ListadoMascotas));
        }

        /// <summary>
        /// Metodo que carga las listas de especies y provincias de la bbdd
        /// </summary>
        public async void CargarLista()
        {
            try
            {
                provincias = new List<clsProvincia>(await clsFirebaseRealtimeBL.GetProvinciaBL());
                tipos = new List<clsTipo>(await clsFirebaseRealtimeBL.GetTipoBL());
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Error al cargar las listas", "Recargar");
                CargarLista();
            }
            NotifyPropertyChanged(nameof(Provincias));
            NotifyPropertyChanged(nameof(Tipos));
        }

        /// <summary>
        /// Metodo que se ejecuta cuando se refresca la dashboard
        /// </summary>
        private async void ActualizarListadoCommand_execute()
        {
            Thread.Sleep(800);
            isRefreshing = true;
            NotifyPropertyChanged(nameof(IsRefreshing));
            try
            {
                listadoMascotas = new ObservableCollection<clsMascota>(await clsFirebaseRealtimeBL.GetPetsBL(null, Preferences.Get("UserToken", "")));
                tipoSeleccionada = null;
                provinciaSeleccionada = null;
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Error al recargar las mascotas", "Recargar");
                ActualizarListadoCommand_execute();
            }
            isRefreshing = false;
            NotifyPropertyChanged(nameof(ListadoMascotas));
            NotifyPropertyChanged(nameof(IsRefreshing));
            NotifyPropertyChanged(nameof(TipoSelecionada));
            NotifyPropertyChanged(nameof(ProvinciaSelecionada));
        }

        /// <summary>
        /// Metodo que carga mas item de los cargados en la dashboard
        /// (No implementado)
        /// </summary>
        private async void CargarMasItemsCommand_execute()
        {
            try
            {
                Thread.Sleep(800);
                //clsMascota m = listadoMascotas.Last();
                //ObservableCollection<clsMascota> listaMasItems = new ObservableCollection<clsMascota>(await clsFirebaseRealtimeBL.GetPetsBL(m.Key));
                listadoMascotas.Union(new ObservableCollection<clsMascota>(await clsFirebaseRealtimeBL.GetPetsBL(listadoMascotas.Last().Key, Preferences.Get("UserToken", ""))));
                tipoSeleccionada = null;
                provinciaSeleccionada = null;
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Error al cargar mas mascotas", "Recargar");
                ActualizarListadoCommand_execute();
            }
            NotifyPropertyChanged(nameof(ListadoMascotas));
        }
        #endregion 
    }
}
