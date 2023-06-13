using BL;
using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallapets.Model.Utilities;

namespace Wallapets.Model
{
    [QueryProperty(nameof(Mascota), "mascota")]
    public class clsEditVM : clsBase
    {
        #region Atributos
        private string link = null;

        private static clsMascota mascota;
        private clsProvincia provinciaSeleccionada;
        private clsTipo tipoSeleccionada;

        private ImageSource imgSource;
        private Stream streamImg = null;

        private DelegateCommand guardarMascotaCommand;
        private DelegateCommand borrarMascotaCommand;
        private DelegateCommand pickImgCommand;

        private IList<clsProvincia> provincias;
        private IList<clsTipo> tipos;
        #endregion

        #region Propiedades
        public clsMascota Mascota
        {
            get { return mascota; }
            set
            {
                mascota = value;
                imgSource = mascota.UrlImagen;
                NotifyPropertyChanged(nameof(Mascota));
                NotifyPropertyChanged(nameof(ImgSource));
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

        public ImageSource ImgSource
        {
            get { return imgSource; }
            set
            {
                imgSource = value;
                NotifyPropertyChanged(nameof(ImgSource));
            }
        }

        public clsProvincia ProvinciaSelecionada
        {
            get { return provinciaSeleccionada; }
            set
            {
                provinciaSeleccionada = value;
                mascota.Provincia = provinciaSeleccionada.Name;
                NotifyPropertyChanged(nameof(ProvinciaSelecionada));
                NotifyPropertyChanged(nameof(Mascota));
            }
        }

        public clsTipo TipoSelecionada
        {
            get { return tipoSeleccionada; }
            set
            {
                tipoSeleccionada = value;
                mascota.Tipo = tipoSeleccionada.Name;
                NotifyPropertyChanged(nameof(TipoSelecionada));
                NotifyPropertyChanged(nameof(Mascota));
            }
        }

        public DelegateCommand GuardarMascotaCommand
        {
            get { return guardarMascotaCommand; }
        }

        public DelegateCommand BorrarMascotaCommand
        {
            get { return borrarMascotaCommand; }
        }

        public DelegateCommand PickImgCommand
        {
            get { return pickImgCommand; }
        }
        #endregion

        public clsEditVM()
        {
            CargarLista();
            guardarMascotaCommand = new DelegateCommand(GuardarMascotaCommand_execute);
            borrarMascotaCommand = new DelegateCommand(BorrarMascotaCommand_execute);
            pickImgCommand = new DelegateCommand(PickImgCommand_Execute);
            mascota = new clsMascota();
        }

        #region Metodos
        /// <summary>
        /// Metodo que selecciona una imagen ya existente en el dispositivo
        /// comprueba si se selecciono imagen o no 
        /// </summary>
        public async void PickImgCommand_Execute()
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Elige una imagen",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                streamImg = await result.OpenReadAsync();
                imgSource = ImageSource.FromStream(() => streamImg);
                try
                {
                    link = await clsFirebaseStorageBL.UploadImageBL(Preferences.Get("UserToken", ""), result.FileName, result.FullPath);
                    imgSource = link;
                } catch (Exception) 
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta", "Imagen no se ha podido subir correctamente", "OK");
                    imgSource = mascota.UrlImagen;
                }
                NotifyPropertyChanged(nameof(ImgSource));
            }
        }
        /// <summary>
        /// Metodo que guarda/acutaliza con los nuevos datos de una mascota
        /// </summary>
        private async void GuardarMascotaCommand_execute()
        {
            bool salida = false;
            try
            {
                if (!mascota.UrlImagen.Equals(link))
                {
                    mascota.UrlImagen = link;
                }
                salida = await clsFirebaseRealtimeBL.UploadPetBL(mascota);
                if (salida)
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta", "Mascota editada correctamente", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta", "La mascota no se ha podido editar", "OK");
                }
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Error en el servidor, intentelo más tarde", "OK");
            }
        }

        /// <summary>
        /// Metodo que borra una mascota segun la clave de la mascota
        /// </summary>
        private async void BorrarMascotaCommand_execute()
        {
            bool borrado = false;
            try
            {
                borrado = await clsFirebaseRealtimeBL.DeletePetBL(mascota.Key);
                if (borrado)
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta", "Mascota borrada correctamente", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta", "La mascota seleccionada no se ha podido borrar", "OK");
                }
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Error en el servidor, intentelo más tarde", "OK");
            }
        }

        /// <summary>
        /// Metodo que carga las listas de provincias y especies rescatadas de la base de datos
        /// </summary>
        public async void CargarLista()
        {
            try
            {
                provincias = new List<clsProvincia>(await clsFirebaseRealtimeBL.GetProvinciaBL());
                tipos = new List<clsTipo>(await clsFirebaseRealtimeBL.GetTipoBL());
                provinciaSeleccionada = provincias.FirstOrDefault(i => i.Name.Equals(mascota.Provincia));
                tipoSeleccionada = tipos.FirstOrDefault(i => i.Name.Equals(mascota.Tipo));
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Error al cargar las opciones", "Recargar");
                CargarLista();
            }
            NotifyPropertyChanged(nameof(Provincias));
            NotifyPropertyChanged(nameof(Tipos));
            NotifyPropertyChanged(nameof(ProvinciaSelecionada));
            NotifyPropertyChanged(nameof(TipoSelecionada));
        }
        #endregion
    }
}
