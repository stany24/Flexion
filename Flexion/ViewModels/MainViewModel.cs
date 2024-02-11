﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using Flexion.Assets.Localization;
using Flexion.Database.Actions;
using Flexion.Logic;
using Flexion.Setting;
using Flexion.Views.Editors.Force;
using Flexion.Views.Editors.Layer;
using Flexion.Views.Editors.Material;
using Flexion.Views.Editors.Piece;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;

namespace Flexion.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly SQLiteConnection _connection;
    public decimal Force { get; set; } = 100;

    public ObservableCollection<Piece> SelectedPiecesMainWindow { get; set; } = new();

    private ObservableCollection<string> _languages;

    public ObservableCollection<string> Languages
    {
        get => _languages;
        set => SetProperty(ref _languages, value);
    }

    private readonly bool _starting = true;
    private string _language;
    public string Language
    {
        get => _language;
        set
        {
            SetProperty(ref _language, value);
            SettingManager.SetLanguage(Language);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Language);
            ChangeLanguage();
        }
    }

    public MainViewModel(SQLiteConnection connection)
    {
        Languages = new ObservableCollection<string>{"fr","en","de"};
        Language = SettingManager.GetLanguage();
        _starting = false;
        _connection = connection;
        SelectedPieces.CollectionChanged += (_,_) => SelectedPieceChanged();
        SelectedLayersOfSelectedPiece.CollectionChanged += (_, _) => SelectedInPieceChanged();
        SelectedAvailableLayers.CollectionChanged += (_, _) => SelectedAvailableChanged();
        DataBaseEvents.LayersChanged += (_, _) => ReloadLayers();
        DataBaseEvents.MaterialsChanged += (_, _) => ReloadMaterials();
        DataBaseEvents.PiecesChanged += (_, _) => ReloadPieces();
        DataBaseEvents.LayerOfPieceChanged += (_, _) => LoadLayersOfPiece(SelectedPieces[0].PieceId);
        ReloadMaterials();
        ReloadLayers();
        ReloadPieces();
        SelectedMaterial = Materials[0];
        
        
        ResourceManager resourceManager = new(typeof(Resources));
        XAxes = new[]
        {
            new Axis
            {
                Name = resourceManager.GetString("LengthWithUnit", new CultureInfo(Language))
            }
        };
        YAxes = new[]
        {
            new Axis
            {
                Name = resourceManager.GetString("DeformationWithUnit", new CultureInfo(Language))
            }
        };
    }

    public void CloseAllWindow()
    {
        _materialEditor?.Close();
        _layerEditor?.Close();
        _pieceEditor?.Close();
        _forceEditor?.Close();
    }

    public Axis[] XAxes { get; set; }
    public Axis[] YAxes { get; set; }
    public ISeries[] SeriesGraphFlexion { get; set; } =
    {
        new LineSeries<ObservablePoint>
        {
            Values = new List<ObservablePoint>
            {
                new(0, 4),
                new(1, 3),
                new(3, 8),
                new(18, 6),
                new(20, 12)
            }
        }
    };
    

    public void CalculateFlexion()
    {
        Task.Run(() =>
        {
            if(SelectedPiecesMainWindow is { Count: 0 }){return;}
            if(SelectedPiecesMainWindow[0].Layers.Count == 0){return;}
            double gap = SelectedPiecesMainWindow[0].Length / 10000;
            IEnumerable<double> values = SelectedPiecesMainWindow[0].CalculateFlexion((int)Force,gap); //returns only NaN caused by CalculateI() in Piece class Caused by Height() in Layer class
            List<ObservablePoint> points = values.Select((t, i) => new ObservablePoint(i, t)).ToList();
            SeriesGraphFlexion[0].Values = points;
        });
    }
    
    private MaterialEditor? _materialEditor;
    public void OpenMaterialEditor()
    {
        if(_materialEditor != null){return;}
        _materialEditor = new MaterialEditor(this);
        _materialEditor.Closed += (_, _) => _materialEditor = null;
        _materialEditor.Show();
    }
    
    private LayerEditor? _layerEditor;
    public void OpenLayerEditor()
    {
        if(_layerEditor != null){return;}
        _layerEditor = new LayerEditor(this);
        _layerEditor.Closed += (_, _) => _layerEditor = null;
        _layerEditor.Show();
    }
    
    private PieceEditor? _pieceEditor;
    public void OpenPieceEditor()
    {
        if(_pieceEditor != null){return;}
        _pieceEditor = new PieceEditor(this);
        _pieceEditor.Closed += (_, _) => _pieceEditor = null;
        _pieceEditor.Show();
    }
    
    private ForceEditor? _forceEditor;
    public void OpenForceEditor()
    {
        if(_forceEditor != null){return;}
        _forceEditor = new ForceEditor(this);
        _forceEditor.Closing += (_, _) => Force = _forceEditor?.CalculateForce() ?? 100;
        _forceEditor.Closed += (_, _) => _forceEditor = null;
        _forceEditor.Show();
    }
}