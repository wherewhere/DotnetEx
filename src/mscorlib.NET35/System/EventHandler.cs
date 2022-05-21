// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System
{
    public delegate void EventHandlerEx<TEventArgs>(object? sender, TEventArgs e); // Removed TEventArgs constraint post-.NET 4
}
