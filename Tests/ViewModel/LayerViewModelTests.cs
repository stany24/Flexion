﻿using BendingCalculator.Database.Actions;
using BendingCalculator.ViewModels;
using Xunit;

namespace Tests.ViewModel;

[Collection("SettingsCollection")]
public class LayerViewModelTests
{
    private readonly MainViewModel _model = new(DataBaseInitializer.InitializeDatabaseConnection());
    
    [Fact]
    public void CreateNewLayerTest()
    {
        int before = _model.Layers.Count;
        _model.CreateNewLayer();
        Assert.Equal(_model.Layers.Count,before+1);
    }

    [Fact]
    public void UpdateLayerTest()
    {

    }
    
    [Fact]
    public void RemoveLayerTest()
    {
        Random rand = new();
        int nbLayer = rand.Next(1, 11);
        for (int i = 0; i < nbLayer; i++)
        {
            _model.CreateNewLayer();
            if (rand.Next(0, 2) != 1) continue;
            _model.SelectedLayer = _model.Layers[^1];
        }
        int finalCount = _model.Layers.Count - 1;
        _model.RemoveLayers();
        Assert.Equal(finalCount,_model.Layers.Count);
    }
}