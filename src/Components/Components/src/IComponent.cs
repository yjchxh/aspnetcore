// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// Represents a UI component.
/// </summary>
public interface IComponent
{
    /// <summary>
    /// Attaches the component to a <see cref="RenderHandle" />.
    /// </summary>
    /// <param name="renderHandle">A <see cref="RenderHandle"/> that allows the component to be rendered.</param>
    void Attach(RenderHandle renderHandle);

    /// <summary>
    /// Sets parameters supplied by the component's parent in the render tree.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <returns>A <see cref="Task"/> that completes when the component has finished updating and rendering itself.</returns>
    /// <remarks>
    /// The <see cref="SetParametersAsync(ParameterView)"/> method should be passed the entire set of parameter values each
    /// time <see cref="SetParametersAsync(ParameterView)"/> is called. It not required that the caller supply a parameter
    /// value for all parameters that are logically understood by the component.
    /// </remarks>
    Task SetParametersAsync(ParameterView parameters);
}
