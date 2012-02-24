﻿using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Caliburn.Micro;
using KimiStudio.BgmOnWp.ViewModels;
using Microsoft.Phone.Controls;

namespace KimiStudio.BgmOnWp
{
    public sealed class AppBootstrapper : PhoneBootstrapper
    {
        private PhoneContainer container;

        protected override void Configure()
        {
            if (RootFrame == null) return;
            container = new PhoneContainer(RootFrame);
            container.RegisterPhoneServices();
            container.PerRequest<MainPageViewModel>();
            container.PerRequest<RecentlyWatchedViewModel>();
            container.PerRequest<SubjectViewModel>();

            AddCustomConventions();
        }

        private static void AddCustomConventions()
        {
            ConventionManager.AddElementConvention<Pivot>(ItemsControl.ItemsSourceProperty, "SelectedItem",
                                                          "SelectionChanged").ApplyBinding =
                (viewModelType, path, property, element, convention) =>
                    {
                        if (ConventionManager
                            .GetElementConvention(typeof (ItemsControl))
                            .ApplyBinding(viewModelType, path, property, element, convention))
                        {
                            ConventionManager
                                .ConfigureSelectedItem(element, Pivot.SelectedItemProperty, viewModelType, path);
                            ConventionManager
                                .ApplyHeaderTemplate(element, Pivot.HeaderTemplateProperty, null, viewModelType);
                            return true;
                        }

                        return false;
                    };

            ConventionManager.AddElementConvention<Panorama>(ItemsControl.ItemsSourceProperty, "SelectedItem",
                                                             "SelectionChanged").ApplyBinding =
                (viewModelType, path, property, element, convention) =>
                    {
                        if (ConventionManager
                            .GetElementConvention(typeof (ItemsControl))
                            .ApplyBinding(viewModelType, path, property, element, convention))
                        {
                            ConventionManager
                                .ConfigureSelectedItem(element, Panorama.SelectedItemProperty, viewModelType, path);
                            ConventionManager
                                .ApplyHeaderTemplate(element, Panorama.HeaderTemplateProperty, null, viewModelType);
                            return true;
                        }

                        return false;
                    };
        }

        protected override PhoneApplicationFrame CreatePhoneApplicationFrame()
        {
            return new TransitionFrame();
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }
    }
}
