using System;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Threading;

namespace PongGame;

public partial class MainWindow : Window
{
    private readonly DispatcherTimer _timer = new();
    private bool _teclaArriba,
        _teclaAbajo,
        _teclaW,
        _teclaS;
    private bool _humano;
    private int _desplazamiento;
    private int _velocidadBolita;
    private float _dxBolita,
        _dyBolita;
    private readonly int _partes = 5;
    private readonly Random _r = new();
    private int _contadorAleatorio;
    private int _posicionIa;
    private int _puntuacionP1,
        _puntuacionP2;
    private double _despl;

    public MainWindow()
    {
        InitializeComponent();
        _puntuacionP1 = 0;
        _puntuacionP2 = 0;
        Iniciar();
    }

    private void Iniciar()
    {
        _contadorAleatorio = 0;
        _posicionIa = 2;
        _humano = true;
        _despl = 0;
        _teclaAbajo = false;
        _teclaArriba = false;
        _teclaW = false;
        _teclaS = false;
        // Console.WriteLine(AreaJuego.Height);
        Canvas.SetLeft(Bolita, 250);
        Canvas.SetTop(Bolita, 220);
        _velocidadBolita = 4;
        _dxBolita = 0;
        _dyBolita = 0;
        _desplazamiento = 9;
        IniciarHilo();
    }

    private void IniciarHilo()
    {
        if (!_timer.IsEnabled)
        {
            _timer.Interval = TimeSpan.FromMilliseconds(25); //.FromSeconds(5);
            _timer.Tick += Juego;
            _timer.Start();
        }
    }

    private void Juego(object? sender, EventArgs e)
    {
        MoverBarras();
        MoverBolita();
    }

    private bool ComprobarColision()
    {
        if (
            Canvas.GetLeft(Bolita) < Canvas.GetLeft(P2) + P2.Width
            && Canvas.GetTop(Bolita) + Bolita.Height > Canvas.GetTop(P2)
            && Canvas.GetTop(Bolita) < Canvas.GetTop(P2) + P2.Height
        )
        {
            Canvas.SetLeft(Bolita, Canvas.GetLeft(P2) + P2.Width - 1);
            RebotarDcha();
            return true;
        }

        if (
            Canvas.GetLeft(Bolita) > Canvas.GetLeft(P1) - P2.Width
            && Canvas.GetTop(Bolita) + Bolita.Height > Canvas.GetTop(P1)
            && Canvas.GetTop(Bolita) < Canvas.GetTop(P1) + P1.Height
        )
        {
            Canvas.SetLeft(Bolita, Canvas.GetLeft(P1) - Bolita.Width + 1);
            RebotarIzda();
            return true;
        }

        return false;
    }

    private void RebotarDcha()
    {
        int partes = 5;
        int posicion = CalcularParte(partes, P2);
        double angulo = 0;
        switch (posicion)
        {
            case 0:
                angulo = Math.PI / 4;
                break;
            case 1:
                angulo = Math.PI / 8;
                break;
            case 2:
                angulo = 0;
                break;
            case 3:
                angulo = -Math.PI / 8;
                break;
            case 4:
                angulo = -Math.PI / 4;
                break;
        }

        _dxBolita = _velocidadBolita * (float)Math.Cos(angulo);
        _dyBolita = -_velocidadBolita * (float)Math.Sin(angulo);
        AumentarVelocidadBolita();
    }

    private int CalcularParte(int v, Rectangle p)
    {
        double distancia = Canvas.GetTop(Bolita) - Bolita.Height - Canvas.GetTop(p);
        double total = p.Height + 2 * Bolita.Height;
        double parte = total / v;
        int posicion = (int)Math.Round(distancia / parte);
        Console.WriteLine(
            "Posicion: "
                + (posicion + 1)
                + "total: "
                + total
                + "PArte: "
                + parte
                + "Distancia: "
                + distancia
        );
        return posicion + 1;
    }

    private void AumentarVelocidadBolita()
    {
        if (_velocidadBolita <= 20)
        {
            _velocidadBolita++;
        }
    }

    private void RebotarIzda()
    {
        int posicion = CalcularParte(_partes, P1);
        double angulo = 0;
        switch (posicion)
        {
            case 0:
                angulo = Math.PI / 4;
                break;
            case 1:
                angulo = Math.PI / 8;
                break;
            case 2:
                angulo = 0;
                break;
            case 3:
                angulo = -Math.PI / 8;
                break;
            case 4:
                angulo = -Math.PI / 4;
                break;
        }

        _dxBolita = -_velocidadBolita * (float)Math.Cos(angulo);
        _dyBolita = -_velocidadBolita * (float)Math.Sin(angulo);
        AumentarVelocidadBolita();
    }

    private void ActualizarLabels()
    {
        LblP1.Content = _puntuacionP1;
        LblP2.Content = _puntuacionP2;
    }

    private void MoverBarras()
    {
        if (_humano)
        {
            if (_teclaArriba)
            {
                if (ComprobarLimiteSuperior(P1))
                {
                    Canvas.SetTop(P1, Canvas.GetTop(P1) - _desplazamiento);
                }
            }

            if (_teclaAbajo)
            {
                if (ComprobarLimiteInferior(P1))
                {
                    Canvas.SetTop(P1, Canvas.GetTop(P1) + _desplazamiento);
                }
            }
        }
        else
        {
            if (_contadorAleatorio % 100 == 0)
            {
                _posicionIa = _r.Next(_partes);
                _contadorAleatorio = 0;
            }

            switch (_posicionIa)
            {
                case 0:
                    _despl = Canvas.GetTop(Bolita);
                    break;
                case 1:
                    _despl = Canvas.GetTop(Bolita) - P1.Height / _partes;
                    break;
                case 2:
                    _despl = Canvas.GetTop(Bolita) - 2 * P1.Height / _partes;
                    ;
                    break;
                case 3:
                    _despl = Canvas.GetTop(Bolita) - 3 * P1.Height / _partes;
                    ;
                    break;
                case 4:
                    _despl = Canvas.GetTop(Bolita) - 4 * P1.Height / _partes;
                    ;
                    break;
            }

            _contadorAleatorio++;
            Canvas.SetTop(P1, _despl);

            if (ComprobarLimiteSuperior(P1)) { }

            if (ComprobarLimiteInferior(P1)) { }
        }

        if (_teclaW)
        {
            if (ComprobarLimiteSuperior(P2))
            {
                Canvas.SetTop(P2, Canvas.GetTop(P2) - _desplazamiento);
            }
        }

        if (_teclaS)
        {
            if (ComprobarLimiteInferior(P2))
            {
                Canvas.SetTop(P2, Canvas.GetTop(P2) + _desplazamiento);
            }
        }
    }

    private bool ComprobarTanto()
    {
        if (Canvas.GetLeft(Bolita) < -2)
        {
            _puntuacionP2++;
            return true;
        }

        if (Canvas.GetLeft(Bolita) > AreaJuego.Width - Bolita.Width)
        {
            _puntuacionP1++;
            return true;
        }

        return false;
    }

    private void MoverBolita()
    {
        if (_dxBolita == 0 && _dyBolita == 0)
        {
            if (_r.Next(2) == 0)
            {
                _dxBolita = _velocidadBolita;
            }
            else
            {
                _dxBolita = -_velocidadBolita;
            }
        }

        if (!ComprobarColision())
        {
            if (ComprobarTanto())
            {
                Iniciar();
                ActualizarLabels();
            }
        }

        ComprobarFuera();
        Canvas.SetTop(Bolita, Canvas.GetTop(Bolita) + _dyBolita);
        Canvas.SetLeft(Bolita, Canvas.GetLeft(Bolita) + _dxBolita);
    }

    private void ComprobarFuera()
    {
        if (Canvas.GetTop(Bolita) + _dyBolita < 0)
        {
            Canvas.SetTop(Bolita, 0);
            _dyBolita = -_dyBolita;
        }

        if (Canvas.GetTop(Bolita) + _dyBolita > AreaJuego.Height - Bolita.Height)
        {
            Canvas.SetTop(Bolita, AreaJuego.Height - Bolita.Height);
            _dyBolita = -_dyBolita;
        }
    }

    private bool ComprobarLimiteInferior(Rectangle p)
    {
        //Console.WriteLine(AreaJuego.Height);
        if (Canvas.GetTop(p) + _desplazamiento > AreaJuego.Height - p.Height)
        {
            Canvas.SetTop(p, AreaJuego.Height - 1 - p.Height);
            return false;
        }

        return true;
    }

    private bool ComprobarLimiteSuperior(Rectangle p)
    {
        if (Canvas.GetTop(p) - _desplazamiento < 0)
        {
            Canvas.SetTop(p, 1);
            return false;
        }

        return true;
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Up:
                _teclaArriba = true;
                break;
            case Key.Down:
                _teclaAbajo = true;
                break;
            case Key.W:
                _teclaW = true;
                break;
            case Key.S:
                _teclaS = true;
                break;
        }
    }

    private void Window_KeyUp(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Up:
                _teclaArriba = false;
                break;
            case Key.Down:
                _teclaAbajo = false;
                break;
            case Key.W:
                _teclaW = false;
                break;
            case Key.S:
                _teclaS = false;
                break;
        }
    }
}
