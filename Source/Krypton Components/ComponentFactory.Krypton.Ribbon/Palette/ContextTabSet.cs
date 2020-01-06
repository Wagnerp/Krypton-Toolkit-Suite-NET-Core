﻿// *****************************************************************************
// BSD 3-Clause License (https://github.com/ComponentFactory/Krypton/blob/master/LICENSE)
//  © Component Factory Pty Ltd, 2006-2020, All rights reserved.
// The software and associated documentation supplied hereunder are the 
//  proprietary information of Component Factory Pty Ltd, 13 Swallows Close, 
//  Mornington, Vic 3931, Australia and are supplied subject to license terms.
// 
//  Modifications by Peter Wagner(aka Wagnerp) & Simon Coghlan(aka Smurf-IV) 2017 - 2020. All rights reserved. (https://github.com/Wagnerp/Krypton-NET-5.490)
//  Version 5.490.0.0  www.ComponentFactory.com
// *****************************************************************************

using System.Drawing;
using System.Diagnostics;
using Krypton.Toolkit;

namespace Krypton.Ribbon
{
    /// <summary>
    /// Stores information needed to draw the display text for a context set
    /// </summary>
    internal class ContextTabSet
    {
        #region Instance Fields

        private ViewDrawRibbonTab _lastTab;

        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the ContextTabSet class.
        /// </summary>
        /// <param name="tab">Reference to first tab of the set.</param>
        /// <param name="context">Reference to owning context details.</param>
        public ContextTabSet(ViewDrawRibbonTab tab,
                             KryptonRibbonContext context)
        {
            Debug.Assert(tab != null);
            Debug.Assert(context != null);

            FirstTab = tab;
            _lastTab = tab;
            Context = context;
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets access to the first tab.
        /// </summary>
        public ViewDrawRibbonTab FirstTab { get; }

        /// <summary>
        /// Gets a value indicating if the tab is the first in set.
        /// </summary>
        /// <param name="tab">Tab to test.</param>
        /// <returns>True if first; otherwise false.</returns>
        public bool IsFirstTab(ViewDrawRibbonTab tab)
        {
            return (tab == FirstTab);
        }

        /// <summary>
        /// Gets a value indicating if the tab is the last in set.
        /// </summary>
        /// <param name="tab">Tab to test.</param>
        /// <returns>True if last; otherwise false.</returns>
        public bool IsLastTab(ViewDrawRibbonTab tab)
        {
            return (tab == _lastTab);
        }

        /// <summary>
        /// Gets a value indicating if the tab is the first or last in set.
        /// </summary>
        /// <param name="tab">Tab to test.</param>
        /// <returns>True if first or last; otherwise false.</returns>
        public bool IsFirstOrLastTab(ViewDrawRibbonTab tab)
        {
            return ((tab == FirstTab) || (tab == _lastTab));
        }

        /// <summary>
        /// Update the last tab in the set with new refernece.
        /// </summary>
        /// <param name="tab">Reference to new last tab.</param>
        public void UpdateLastTab(ViewDrawRibbonTab tab)
        {
            Debug.Assert(tab != null);
            _lastTab = tab;
        }

        /// <summary>
        /// Gets the left position needed to show the context tab in screen coordinates.
        /// </summary>
        /// <returns>Screen position.</returns>
        public Point GetLeftScreenPosition()
        {
            Point ret = new Point(FirstTab.ClientLocation.X - 1, FirstTab.ClientLocation.Y);

            if (FirstTab.OwningControl != null)
            {
                ret = FirstTab.OwningControl.PointToScreen(ret);
            }

            return ret;
        }

        /// <summary>
        /// Gets the right position needed to show the context tab in screen coordinates.
        /// </summary>
        /// <returns>Screen position.</returns>
        public Point GetRightScreenPosition()
        {
            Point ret = new Point(_lastTab.ClientRectangle.Right + 1, _lastTab.ClientLocation.Y);

            if (_lastTab.OwningControl != null)
            {
                ret = _lastTab.OwningControl.PointToScreen(ret);
            }

            return ret;
        }

        /// <summary>
        /// Gets the context component.
        /// </summary>
        public KryptonRibbonContext Context { get; }

        /// <summary>
        /// Gets the name of the context.
        /// </summary>
        public string ContextName => Context.ContextName;

        /// <summary>
        /// Gets the name of the context.
        /// </summary>
        public Color ContextColor => Context.ContextColor;

        /// <summary>
        /// Gets the title of the context.
        /// </summary>
        public string ContextTitle => Context.ContextTitle;

        #endregion
    }

    /// <summary>
    /// Specialise the generic collection with type specific rules for item accessor.
    /// </summary>
    internal class ContextTabSetCollection : TypedCollection<ContextTabSet>
    {
        #region Public
        /// <summary>
        /// Gets the item with the provided unique name.
        /// </summary>
        /// <param name="name">Name of the ribbon context instance.</param>
        /// <returns>Item at specified index.</returns>
        public override ContextTabSet this[string name]
        {
            get
            {
                // Search for a context with the same name as that requested.
                foreach (ContextTabSet context in this)
                {
                    if (context.ContextName == name)
                    {
                        return context;
                    }
                }

                // Let base class perform standard processing
                return base[name];
            }
        }
        #endregion
    }
}
