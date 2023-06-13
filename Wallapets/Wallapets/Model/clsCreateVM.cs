using BL;
using ENTITIES;
using Wallapets.Model.Utilities;

namespace Wallapets.Model
{
    public class clsCreateVM : clsBase
    {
        #region Propiedades
        private ImageSource imgSource = "img_add.png";
        private Stream streamImg = null;
        private string link = null;
        private string nameImg = null;
        private string imgPath = null;
        private string titulo;
        private string descripcion;
        private IList<clsProvincia> provincias;
        private IList<clsTipo> tipos;
        private clsProvincia provinciaSeleccionada;
        private clsTipo tipoSeleccionada;
        private string localidad;
        private DelegateCommand pickImgCommand;
        private DelegateCommand uploadAnimalCommand;
        #endregion

        #region Atributos
        public ImageSource ImgSource
        {
            get { return imgSource; }
            set
            {
                imgSource = value;
                NotifyPropertyChanged(nameof(ImgSource));
            }
        }
        public string Titulo
        {
            get { return titulo; }
            set
            {
                titulo = value;
                NotifyPropertyChanged(nameof(Titulo));
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

        public string Localidad
        {
            get { return localidad; }
            set
            {
                localidad = value;
                NotifyPropertyChanged(nameof(Localidad));
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

        public string Descripcion
        {
            get => descripcion;
            set
            {
                descripcion = value;
                NotifyPropertyChanged(nameof(Descripcion));
            }
        }

        public DelegateCommand PickImgCommand
        {
            get { return pickImgCommand; }
        }

        public DelegateCommand UploadAnimalCommand
        {
            get { return uploadAnimalCommand; }
        }
        #endregion
        public clsCreateVM()
        {
            pickImgCommand = new DelegateCommand(PickImgCommand_Execute);
            uploadAnimalCommand = new DelegateCommand(UploadAnimalCommand_ExecuteAsync);
            CargarLista();
        }
        #region Commands
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
                nameImg = result.FileName;
                imgPath = result.FullPath;
                imgSource = ImageSource.FromStream(() => streamImg);
                NotifyPropertyChanged(nameof(ImgSource));
            }
        }

        /// <summary>
        /// Metodo que sube una mascota a la base de datos
        /// </summary>
        public async void UploadAnimalCommand_ExecuteAsync()
        {
            bool salida = false;
            try
            {
                //Primero sube la imagen y obtiene la url de ella
                link = await clsFirebaseStorageBL.UploadImageBL(Preferences.Get("UserToken", ""), nameImg, imgPath);
                //Despues rescata todos los campos e incluye la imagen y el uid del user
                clsMascota pet = new clsMascota(titulo, descripcion, provinciaSeleccionada.Name, localidad, tipoSeleccionada.Name, link, Preferences.Get("UserToken", ""));
                //Sube la mascota, segun el resultado dara error o no
                salida = await clsFirebaseRealtimeBL.UploadPetBL(pet);
                //Si la respuesta es true, dira que la mascota se ha creado correctamente,
                //borrara todos los datos del formulario y volvera a la dashboard
                if (salida)
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta", "Mascota creada correctamente", "OK");
                    imgSource = "img_add.png";
                    titulo = "";
                    descripcion = "";
                    provinciaSeleccionada = null;
                    localidad = "";
                    tipoSeleccionada = null;
                    NotifyPropertyChanged(nameof(ImgSource));
                    NotifyPropertyChanged(nameof(Titulo));
                    NotifyPropertyChanged(nameof(Descripcion));
                    NotifyPropertyChanged(nameof(ProvinciaSelecionada));
                    NotifyPropertyChanged(nameof(Localidad));
                    NotifyPropertyChanged(nameof(TipoSelecionada));
                    await Shell.Current.GoToAsync(state: "//Dashboard");
                } else
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta", "La mascota no se ha podido subir", "OK");
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
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Error al cargar las opciones", "Recargar");
                CargarLista();
            }
            NotifyPropertyChanged(nameof(Provincias));
            NotifyPropertyChanged(nameof(Tipos));
        }
        #endregion

    }
}
