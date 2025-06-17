using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace TicTacToe;

public partial class MainWindow : Window
{
    private bool _turno;
    private readonly int[] _partida = new int[9];

    private int _victorias1 = 0,
        _victorias2 = 0;

    private readonly List<Ellipse> _circulos = new();
    private readonly List<Grid> _aspas = new();

    public MainWindow()
    {
        InitializeComponent();
        CargarLista();
        Inicio();
    }

    private void CargarLista()
    {
        _circulos.Add(C1);
        _circulos.Add(C2);
        _circulos.Add(C3);
        _circulos.Add(C4);
        _circulos.Add(C5);
        _circulos.Add(C6);
        _circulos.Add(C7);
        _circulos.Add(C8);
        _circulos.Add(C9);
        _aspas.Add(X1);
        _aspas.Add(X2);
        _aspas.Add(X3);
        _aspas.Add(X4);
        _aspas.Add(X5);
        _aspas.Add(X6);
        _aspas.Add(X7);
        _aspas.Add(X8);
        _aspas.Add(X9);
    }

    private void Inicio()
    {
        foreach (Ellipse circulo in _circulos)
        {
            circulo.IsVisible = false;
        }

        foreach (Grid aspa in _aspas)
        {
            aspa.IsVisible = false;
        }

        lblTurno.Content = "Jugador 1";
        _turno = true;

        for (int i = 0; i < 9; i++)
        {
            _partida[i] = 0;
        }
    }

    private void Tablero_MouseLeftButtonDown(object? sender, PointerPressedEventArgs e)
    {
        Control? fuente = Tablero.GetVisualAt(e.GetPosition(Tablero)) as Control;
        switch (fuente?.Name)
        {
            case "Rect1":
                Mostrar(1);
                //MessageBox.Show("0,0");
                break;
            case "Rect2":
                Mostrar(2);
                //MessageBox.Show("0,1");
                break;
            case "Rect3":
                Mostrar(3);
                // MessageBox.Show("0,2");
                break;
            case "Rect4":
                Mostrar(4);
                //MessageBox.Show("1,0");
                break;
            case "Rect5":
                Mostrar(5);
                //MessageBox.Show("1,1");
                break;
            case "Rect6":
                Mostrar(6);
                //MessageBox.Show("1,2");
                break;
            case "Rect7":
                Mostrar(7);
                //MessageBox.Show("2,0");
                break;
            case "Rect8":
                Mostrar(8);
                //MessageBox.Show("2,1");
                break;
            case "Rect9":
                Mostrar(9);
                //MessageBox.Show("2,2");
                break;
        }
    }

    private void Mostrar(int cuadradito)
    {
        if (_partida[cuadradito - 1] != 0)
        {
            return;
        }

        if (_turno)
        {
            // turno del jugador 1
            _partida[cuadradito - 1] = 1;
            _aspas[cuadradito - 1].IsVisible = true;
        }
        else
        {
            //turno del jugador 2
            _partida[cuadradito - 1] = 2;
            _circulos[cuadradito - 1].IsVisible = true;
        }

        if (ComprobarFinal())
        {
            return;
        }

        if (_turno)
        {
            lblTurno.Content = "Jugador 2";
        }
        else
        {
            lblTurno.Content = "Jugador 1";
        }

        _turno = !_turno;
    }

    private bool ComprobarFinal()
    {
        if (
            (_partida[0] == 1 && _partida[1] == 1 && _partida[2] == 1)
            || (_partida[3] == 1 && _partida[4] == 1 && _partida[5] == 1)
            || (_partida[6] == 1 && _partida[7] == 1 && _partida[8] == 1)
            || (_partida[0] == 1 && _partida[3] == 1 && _partida[6] == 1)
            || (_partida[1] == 1 && _partida[4] == 1 && _partida[7] == 1)
            || (_partida[2] == 1 && _partida[5] == 1 && _partida[8] == 1)
            || (_partida[0] == 1 && _partida[4] == 1 && _partida[8] == 1)
            || (_partida[2] == 1 && _partida[4] == 1 && _partida[6] == 1)
        )
        {
            Victorias1.Content = ++_victorias1;
            // MessageBox.Show("Gana el jugador 1");
            Inicio();
            return true;
        }

        if (
            (_partida[0] == 2 && _partida[1] == 2 && _partida[2] == 2)
            || (_partida[3] == 2 && _partida[4] == 2 && _partida[5] == 2)
            || (_partida[6] == 2 && _partida[7] == 2 && _partida[8] == 2)
            || (_partida[0] == 2 && _partida[3] == 2 && _partida[6] == 2)
            || (_partida[1] == 2 && _partida[4] == 2 && _partida[7] == 2)
            || (_partida[2] == 2 && _partida[5] == 2 && _partida[8] == 2)
            || (_partida[0] == 2 && _partida[4] == 2 && _partida[8] == 2)
            || (_partida[2] == 2 && _partida[4] == 2 && _partida[6] == 2)
        )
        {
            _victorias2++;
            Victorias2.Content = _victorias2;
            // MessageBox.Show("Gana el jugador 2");
            Inicio();
            return true;
        }

        bool final = true;
        for (int i = 0; i < 9 && final; i++)
        {
            if (_partida[i] == 0)
            {
                final = false;
            }
        }

        if (final)
        {
            // MessageBox.Show("Gana el gato");
            Inicio();
            return true;
        }

        return false;
    }

    private void Tablero_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            Tablero_MouseLeftButtonDown(sender, e);
        }
    }
}
