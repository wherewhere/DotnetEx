// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System
{
    /// <summary>
    /// Encapsulates a method that has no parameters and returns a value of the type specified by the TResult parameter.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
    /// <returns>The return value of the method that this delegate encapsulates.</returns>
    public delegate TResult FuncEx<out TResult>();
}