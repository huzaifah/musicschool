using System.Text.Json;
using BlazorApp.Models;
using BlazorApp.Services.Interfaces;
using Microsoft.JSInterop;

namespace BlazorApp.Services.Implementation;

/// <summary>
/// Perkhidmatan untuk menguruskan keadaan borang dalam localStorage
/// </summary>
public class FormStateService : IFormStateService
{
    private readonly IJSRuntime _jsRuntime;
    private const string FormStateKey = "nadiritma_form_state";
    private const string CurrentStepKey = "nadiritma_current_step";

    public FormStateService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SaveFormStateAsync(RegistrationFormModel model)
    {
        try
        {
            var json = JsonSerializer.Serialize(model);
            await _jsRuntime.InvokeVoidAsync("formState.save", FormStateKey, json);
        }
        catch (InvalidOperationException)
        {
            // Prerendering - localStorage not available
        }
    }

    public async Task<RegistrationFormModel?> LoadFormStateAsync()
    {
        try
        {
            var json = await _jsRuntime.InvokeAsync<string?>("formState.load", FormStateKey);
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            return JsonSerializer.Deserialize<RegistrationFormModel>(json);
        }
        catch (InvalidOperationException)
        {
            // Prerendering - localStorage not available
            return null;
        }
    }

    public async Task ClearFormStateAsync()
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("formState.remove", FormStateKey);
            await _jsRuntime.InvokeVoidAsync("formState.remove", CurrentStepKey);
        }
        catch (InvalidOperationException)
        {
            // Prerendering - localStorage not available
        }
    }

    public async Task SaveCurrentStepAsync(int step)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("formState.save", CurrentStepKey, step.ToString());
        }
        catch (InvalidOperationException)
        {
            // Prerendering - localStorage not available
        }
    }

    public async Task<int> LoadCurrentStepAsync()
    {
        try
        {
            var stepStr = await _jsRuntime.InvokeAsync<string?>("formState.load", CurrentStepKey);
            if (int.TryParse(stepStr, out var step))
            {
                return step;
            }
        }
        catch (InvalidOperationException)
        {
            // Prerendering - localStorage not available
        }

        return 1;
    }
}
