using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Layout;
using DynamicData;
using FlexionV2.Logic;
using FlexionV2.ViewModels;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;

namespace FlexionV2.Views;

public partial class MainWindow : Window
{
    private double Ecart = 1e-4;
    public MainWindow()
    {
        InitializeComponent();
        InitializeUi();
    }

    private void InitializeUi()
    {
        InitializeForceArea();
        InitializePieceArea();
        InitializeLayerArea();
        InitializeMaterialArea();
    }

    private void InitializeForceArea()
    {
        TextBlock lblForce = new() { Text = "Force:",VerticalAlignment = VerticalAlignment.Center };
        Grid.SetColumn(lblForce,0);
        Grid.SetRow(lblForce,0);
        GridForce.Children.Add(lblForce);
        NumericUpDown nudForce = new() { Value = 5 };
        Grid.SetColumn(nudForce,0);
        Grid.SetRow(nudForce,2);
        GridForce.Children.Add(nudForce);
        Button btnForce = new() { Content = "Modifier" };
        Grid.SetColumn(btnForce,2);
        Grid.SetRow(btnForce,0);
        GridForce.Children.Add(btnForce);
        Button btnStart = new() { Content = "Commencer" };
        btnStart.Click += (_, _) => Task.Run(CalculateFlexion);
        Grid.SetColumn(btnStart,0);
        Grid.SetRow(btnStart,4);
        GridForce.Children.Add(btnStart);
    }

    private void InitializePieceArea()
    {
        TextBlock lblPiece = new() { Text = "Piece:",VerticalAlignment = VerticalAlignment.Center };
        Grid.SetColumn(lblPiece,0);
        Grid.SetRow(lblPiece,0);
        GridPiece.Children.Add(lblPiece);
        ListBox lbxPiece = new(){VerticalAlignment = VerticalAlignment.Stretch,HorizontalAlignment = HorizontalAlignment.Stretch};
        Grid.SetColumn(lbxPiece,0);
        Grid.SetRow(lbxPiece,2);
        GridPiece.Children.Add(lbxPiece);
        Button btnPiece = new() { Content = "Modifier" };
        Grid.SetColumn(btnPiece,2);
        Grid.SetRow(btnPiece,0);
        GridPiece.Children.Add(btnPiece);
    }

    private void InitializeLayerArea()
    {
        TextBlock lblLayer = new() { Text = "Layer:",VerticalAlignment = VerticalAlignment.Center};
        Grid.SetColumn(lblLayer,0);
        Grid.SetRow(lblLayer,0);
        GridLayer.Children.Add(lblLayer);
        ListBox lbxLayer = new(){VerticalAlignment = VerticalAlignment.Stretch,HorizontalAlignment = HorizontalAlignment.Stretch};
        Grid.SetColumn(lbxLayer,0);
        Grid.SetRow(lbxLayer,2);
        GridLayer.Children.Add(lbxLayer);
        Button btnLayer = new() { Content = "Modifier" };
        Grid.SetColumn(btnLayer,2);
        Grid.SetRow(btnLayer,0);
        GridLayer.Children.Add(btnLayer);
    }

    private void InitializeMaterialArea()
    {
        TextBlock lblMaterial = new() { Text = "Material:",VerticalAlignment = VerticalAlignment.Center };
        Grid.SetColumn(lblMaterial,0);
        Grid.SetRow(lblMaterial,0);
        GridMaterial.Children.Add(lblMaterial);
        ListBox lbxMaterial = new(){VerticalAlignment = VerticalAlignment.Stretch,HorizontalAlignment = HorizontalAlignment.Stretch};
        Grid.SetColumn(lbxMaterial,0);
        Grid.SetRow(lbxMaterial,2);
        GridMaterial.Children.Add(lbxMaterial);
        Button btnMaterial = new() { Content = "Modifier" };
        Grid.SetColumn(btnMaterial,2);
        Grid.SetRow(btnMaterial,0);
        GridMaterial.Children.Add(btnMaterial);
    }

    private void CalculateFlexion()
    {
        Piece piece = new(1,"test");
        Material material = new("test",69e9);
        piece.Layers.Add(new Layer(material,5,5));
        FillGraph(piece.Intégrale(500, Ecart));
    }

    private void FillGraph(double[] data)
    {
        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => {
            if(DataContext is not MainWindowViewModel model)  {return;}

            LineSeries<ObservablePoint> line = new();
            List<ObservablePoint> values = data.Select((t, i) => new ObservablePoint(i, t)).ToList();

            line.Values = values;
            model.Series[0]=line ;
        });
        ChartResult.CoreChart.Update(new ChartUpdateParams { IsAutomaticUpdate = false, Throttling = false });
    }
}