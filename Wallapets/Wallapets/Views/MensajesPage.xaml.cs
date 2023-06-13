namespace Wallapets.Views;

using System.Threading.Tasks;
using System.Timers;
using Wallapets.Model;

public partial class MensajesPage : ContentPage
{
    private Timer _timer;

    /// <summary>
    /// Inicia la pagina y crea y temporizador
    /// para que cada 1,5 seg busque si hay nuevos mensajes
    /// disponibles
    /// </summary>
    public MensajesPage()
	{
		InitializeComponent();
        _timer = new Timer(1500); // 1,5 segundos = 1500 milisegundos
        _timer.Elapsed += TimerElapsedAsync;
        _timer.AutoReset = true;
        _timer.Start();
    }

    /// <summary>
    /// Metodo que comprueba cada 1.5sg si hay nuevos mensajes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TimerElapsedAsync(object sender, ElapsedEventArgs e)
    {
        var binding = (clsMensajesVM)this.BindingContext;
        binding.CargarMensajes();
        Application.Current.Dispatcher.Dispatch(() =>
        {
            //Si hay nuevos mensajes, Res sera true
            if (binding.Res)
            {
                //Cogera el ultimo elemento de la lista y hara un scrollto dicho elemento
                ListadoMensajes.ScrollTo(binding.ListadoMensajes.Last(), position: ScrollToPosition.End, animate: true);
                //Pondra res a false para que no se cree bucle infinito
                binding.Res = false;
            }
        });
    }

    /// <summary>
    /// Metodo que cargar los mensajes en pantalla
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        var binding = (clsMensajesVM)this.BindingContext;
        binding.CargarMensajes();
    }

    /// <summary>
    /// Metodo que cuando sales de la pagina se para el tiempo
    /// </summary>
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _timer.Stop();
        _timer.Elapsed -= TimerElapsedAsync;
    }
}