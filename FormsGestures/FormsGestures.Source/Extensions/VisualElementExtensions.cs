﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;
using P42.Utils;
using System.Collections;

namespace FormsGestures
{
    /// <summary>
    /// Xamarin.Forms.VisualElement extension methods
    /// </summary>
    public static class VisualElementExtensions
    {
        //public static readonly BindableProperty InterceptGesturesProperty = BindableProperty.Create("InterceptGestures",typeof(bool),typeof(Listener),false);

        /*
		public static readonly BindableProperty IgnoreChildrenTouchesProperty = BindableProperty.Create("IngoreChildrenTouches",typeof(bool),typeof(VisualElement),false);

		public static void set_IgnoreChildrenTouches(this VisualElement element, bool ignore) {
			element.SetValue (IgnoreChildrenTouchesProperty, ignore);
		}

		public static bool get_IgnoreChildrenTouches(this VisualElement element) {
			return (bool)element.GetValue (IgnoreChildrenTouchesProperty);
		}
		*/


        static ICoordTransform _service;
        static ICoordTransform Service
        {
            get
            {
                if (_service == null)
                    _service = DependencyService.Get<ICoordTransform>();
                return _service;
            }
        }


        // THIS MAY NOT WORK WITH UWP .NET NATIVE COMPILER CHAIN
        /// <summary>
        /// Is this element a descendent of an ancestor element?
        /// </summary>
        /// <param name="child"></param>
        /// <param name="ancestor"></param>
        /// <returns></returns>
        public static bool IsDescendentOf(this Element child, Element ancestor)
        {
            //if (child == ancestor)
            //	return true;
            if (child.Parent == ancestor)
                return true;
            return child.Parent != null && child.Parent.IsDescendentOf(ancestor);
        }

        /// <summary>
        /// Is this element an ancestor or a descendent element?
        /// </summary>
        /// <param name="ancestor"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        public static bool IsAncestorOf(this Element ancestor, Element child)
        {
            return child.IsDescendentOf(ancestor);
        }

        /// <summary>
        /// Translates the bounds of an element to the coordinates of app's window
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Rectangle BoundsToWinCoord(this VisualElement element)
        {
            return element.BoundsToEleCoord(Application.Current.MainPage);
        }

        /// <summary>
        /// Translates the bounds of an element to the coordinates of another, reference element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="referenceElement"></param>
        /// <returns></returns>
        public static Rectangle BoundsToEleCoord(this VisualElement element, VisualElement referenceElement)
        {
            return Service.CoordTransform(element, element.Bounds, referenceElement);
        }

        /// <summary>
        /// Translates the location of an element to the app's window's coordinates
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Point LocationToWinCoord(this VisualElement element)
        {
            return element.LocationToEleCoord(Application.Current.MainPage);
        }

        /// <summary>
        /// Translates the location of an element to the coordinates of another, reference element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="referenceElement"></param>
        /// <returns></returns>
        public static Point LocationToEleCoord(this VisualElement element, VisualElement referenceElement)
        {
            return Service.CoordTransform(element, element.Bounds.Location, referenceElement);
        }

        /// <summary>
        /// determines if point in this element is withing the bounds of another, test element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="p"></param>
        /// <param name="testElement"></param>
        /// <returns></returns>
        public static bool HitTest(this VisualElement element, Point p, VisualElement testElement)
        {
            var testPoint = CoordTransform(element, p, testElement);
            return testElement.Bounds.Contains(testPoint);
        }

        /// <summary>
        /// translates a point in this element's coordinate space to the app's window's coordintate space
        /// </summary>
        /// <param name="element"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Point ToWinCoord(this VisualElement element, Point p)
        {
            return element.ToEleCoord(p, Application.Current.MainPage);
        }

        /// <summary>
        /// translates a point in this element's coordinate space to the coordinate space of another, reference element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="p"></param>
        /// <param name="altElement"></param>
        /// <returns></returns>
        public static Point ToEleCoord(this VisualElement element, Point p, VisualElement altElement)
        {
            return Service.CoordTransform(element, p, altElement);
        }

        /// <summary>
        /// translates a point in the app's window coordinate space to that of this element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Point WinToEleCoord(this VisualElement element, Point p)
        {
            return Service.CoordTransform(Application.Current.MainPage, p, element);
        }
        /// <summary>
        /// translates a rectangle from the app's window coordinates to that of this element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="windowRect"></param>
        /// <returns></returns>
        public static Rectangle WinToEleCoord(this VisualElement element, Rectangle windowRect)
        {
            return Service.CoordTransform(Application.Current.MainPage, windowRect, element);
        }

        /// <summary>
        /// translates a point from the coordinates of this element to that of another
        /// </summary>
        /// <param name="fromElement"></param>
        /// <param name="p"></param>
        /// <param name="toElement"></param>
        /// <returns></returns>
        public static Point CoordTransform(VisualElement fromElement, Point p, VisualElement toElement)
        {
            return Service.CoordTransform(fromElement, p, toElement);
        }
        /// <summary>
        /// translates a rectangle from the coordinates of this element to that of another
        /// </summary>
        /// <param name="fromElement"></param>
        /// <param name="r"></param>
        /// <param name="toElement"></param>
        /// <returns></returns>
        public static Rectangle CoordTransform(VisualElement fromElement, Rectangle r, VisualElement toElement)
        {
            return Service.CoordTransform(fromElement, r, toElement);
        }

        /// <summary>
        /// Gets or creates a FormsGestures.Listener for a Xamarin.Forms.VisualElement
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Listener GestureListener(this VisualElement element)
        {
            return Listener.For(element);
        }

        /// <summary>
        /// Enumerates all of the children of a parent element of a given type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parentElement"></param>
        /// <param name="propertyName"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static List<T> FindChildrenWithPropertyAndOfType<T>(this VisualElement parentElement, string propertyName, List<T> result = null) where T : VisualElement
        {
            result = result ?? new List<T>();

            var content = parentElement.GetPropertyValue("Content") as VisualElement;
            var children = parentElement.GetPropertyValue("Children") as IEnumerable;

            if (children == null)
            {
                if (content != null)
                    FindChildrenWithPropertyAndOfType<T>(content, propertyName, result);
            }
            else
            {
                foreach (var child in children)
                {
                    if (child is VisualElement visualElement)
                        FindChildrenWithPropertyAndOfType<T>(visualElement, propertyName, result);
                }
            }

            if (parentElement is T && propertyName == null || parentElement.HasProperty(propertyName))
                result.Add(parentElement as T);

            return result;
        }

        /// <summary>
        /// Enumarates all the chilren of a VisualElement with a given property name
        /// </summary>
        /// <param name="parentElement"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static List<VisualElement> FindChildrenWithProperty(this VisualElement parentElement, string propertyName)
        {
            return FindChildrenWithPropertyAndOfType<VisualElement>(parentElement, propertyName);
        }

        /// <summary>
        /// Enumarates all the chilren of a VisualElement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parentElement"></param>
        /// <returns></returns>
        public static List<T> FindVisualElementsOfType<T>(this VisualElement parentElement) where T : VisualElement
        {
            return FindChildrenWithPropertyAndOfType<T>(parentElement, null);
        }

        /// <summary>
        /// Enumarates all VisualElements with a given type (T) and property name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static List<T> FindVisualElementsWithPropertyAndOfType<T>(string propertyName) where T : VisualElement
        {
            return FindChildrenWithPropertyAndOfType<T>(Xamarin.Forms.Application.Current.MainPage, propertyName);
        }

        /// <summary>
        /// Enumarates all VisualElements with a given property name
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static List<VisualElement> FindVisualElementsWithProperty(string propertyName)
        {
            return FindChildrenWithPropertyAndOfType<VisualElement>(Xamarin.Forms.Application.Current.MainPage, propertyName);
        }

        /// <summary>
        /// Enumarates all VisualElements with a given type (T) 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> FindVisualElementsOfType<T>() where T : VisualElement
        {
            return FindChildrenWithPropertyAndOfType<T>(Xamarin.Forms.Application.Current.MainPage, null);
        }


    }
}

