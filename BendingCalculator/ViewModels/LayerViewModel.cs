﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BendingCalculator.Database.Actions;
using BendingCalculator.Logic.Math;

namespace BendingCalculator.ViewModels;

public partial class MainViewModel
{
    #region Bindings

    private ObservableCollection<Layer> _layers = new();
    public ObservableCollection<Layer> Layers { 
        get => _layers;
        set => SetProperty(ref _layers, value);
    }
    
    private Layer? _selectedLayer;
    public Layer? SelectedLayer
    {
        get => _selectedLayer;
        set
        {
            SetProperty(ref _selectedLayer, value);
            SelectedLayerChanged();
        }
    }

    private bool _uiEnabledLayerEditor;
    public bool UiEnabledLayerEditor { 
        get => _uiEnabledLayerEditor;
        set => SetProperty(ref _uiEnabledLayerEditor, value);
    }

    private double _widthSide;

    public double WidthSide
    {
        get => _widthSide;
        set
        {
            SetProperty(ref _widthSide, value);
            ChangeWidthSide();
        }
    }
    
    private double _widthCenter;

    public double WidthCenter
    {
        get => _widthCenter;
        set
        {
            SetProperty(ref _widthCenter, value);
            ChangeWidthCenter();
        }
    }
    
    private double _heightSide;

    public double HeightSide
    {
        get => _heightSide;
        set
        {
            SetProperty(ref _heightSide, value);
            ChangeHeightSide();
        }
    }
    
    private double _heightCenter;

    public double HeightCenter
    {
        get => _heightCenter;
        set
        {
            SetProperty(ref _heightCenter, value);
            ChangeHeightCenter();
        }
    }

    private Material? _selectedMaterial;
    public Material? SelectedMaterial
    {
        get => _selectedMaterial;
        set
        {
            SetProperty(ref _selectedMaterial, value);
            MaterialChanged();
        }
    }

    #endregion

    #region Layer Edition

    public void CreateNewLayer()
    {
        Material? material = null;
        if (SelectedMaterial is not null)
        {
            material = SelectedMaterial;
        }
        else
        {
            if (Materials.Count > 0)
            {
                material = Materials[0];
            }
        }
        Layer layer = new(material , 0.045,0.045, 0.01, 0.01);
        DataBaseCreator.NewLayer(_connection,layer);
        SelectedLayer = Layers[^1];
    }
    
    public void RemoveLayers()
    {
        if(SelectedLayer == null){return;}
        DataBaseRemover.RemoveLayer(_connection,SelectedLayer.LayerId);
        SelectedLayer = null;
    }
    
    private void ChangeWidthSide()
    {
        if(SelectedLayer == null){return;}
        SelectedLayer.WidthOnSides = WidthSide / 1000;
        DataBaseUpdater.UpdateLayers(_connection,SelectedLayer);
    }

    private void ChangeWidthCenter()
    {
        if(SelectedLayer == null){return;}
        SelectedLayer.WidthAtCenter = WidthCenter/ 1000;
        DataBaseUpdater.UpdateLayers(_connection,SelectedLayer);
    }

    private void ChangeHeightSide()
    {
        if(SelectedLayer == null){return;}
        SelectedLayer.HeightOnSides = HeightSide/ 1000;
        DataBaseUpdater.UpdateLayers(_connection,SelectedLayer);
    }

    private void ChangeHeightCenter()
    {
        if(SelectedLayer == null){return;}
        SelectedLayer.HeightAtCenter = HeightCenter/ 1000;
        DataBaseUpdater.UpdateLayers(_connection,SelectedLayer);
    }

    private void MaterialChanged()
    {
        if(SelectedLayer == null){return;}
        SelectedLayer.Material = SelectedMaterial;
        DataBaseUpdater.UpdateLayers(_connection,SelectedLayer);
    }
    
    private void ReloadLayers()
    {
        List<Layer> layers = DataBaseLoader.LoadLayers(_connection);
        while (layers.Count != Layers.Count)
        {
            if (layers.Count < Layers.Count)
            {
                Layers.RemoveAt(0);
            }
            else
            {
                Layers.Add(new Layer());
            }
        }

        for (int i = 0; i < layers.Count; i++)
        {
            Layers[i].LayerId = layers[i].LayerId;
            Layers[i].Material = layers[i].Material;
            Layers[i].HeightAtCenter = layers[i].HeightAtCenter;
            Layers[i].HeightOnSides = layers[i].HeightOnSides;
            Layers[i].WidthAtCenter = layers[i].WidthAtCenter;
            Layers[i].WidthOnSides = layers[i].WidthOnSides;
        }
    }
    
    private void SelectedLayerChanged()
    {
        if(SelectedLayer == null)
        {
            UiEnabledLayerEditor = false;
            HeightCenter = 0;
            HeightSide = 0;
            WidthCenter = 0;
            WidthSide = 0;
            SelectedMaterial = null;
            return;
        }
        UiEnabledLayerEditor = true;
        SelectedMaterial = SelectedLayer.Material is not null ? Materials.FirstOrDefault(m => m.MaterialId == SelectedLayer.Material.MaterialId) : null;
        HeightCenter = SelectedLayer.HeightAtCenter*1000;
        HeightSide = SelectedLayer.HeightOnSides*1000;
        WidthCenter = SelectedLayer.WidthAtCenter*1000;
        WidthSide = SelectedLayer.WidthOnSides*1000;
    }

    #endregion
}