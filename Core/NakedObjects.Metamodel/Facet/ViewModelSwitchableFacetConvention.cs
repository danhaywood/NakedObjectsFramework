// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using Common.Logging;
using NakedObjects.Architecture.Adapter;
using NakedObjects.Architecture.Component;
using NakedObjects.Architecture.Facet;
using NakedObjects.Architecture.Spec;
using NakedObjects.Core;
using NakedObjects.Core.Util;

namespace NakedObjects.Meta.Facet {
    [Serializable]
    public sealed class ViewModelSwitchableFacetConvention : ViewModelFacetAbstract {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ViewModelSwitchableFacetConvention));

        public ViewModelSwitchableFacetConvention(ISpecification holder) : base(Type, holder) {}

        private static Type Type => typeof(IViewModelFacet);

        public override string[] Derive(INakedObjectAdapter nakedObjectAdapter, INakedObjectManager nakedObjectManager, IDomainObjectInjector injector) {
            return nakedObjectAdapter.GetDomainObject<IViewModel>().DeriveKeys();
        }

        public override void Populate(string[] keys, INakedObjectAdapter nakedObjectAdapter, INakedObjectManager nakedObjectManager, IDomainObjectInjector injector) {
            nakedObjectAdapter.GetDomainObject<IViewModel>().PopulateUsingKeys(keys);
        }

        public override bool IsEditView(INakedObjectAdapter nakedObjectAdapter) {
            var target = nakedObjectAdapter.GetDomainObject<IViewModelSwitchable>();

            if (target != null) {
                return target.IsEditView();
            }

            throw new NakedObjectSystemException(Log.LogAndReturn(nakedObjectAdapter.Object == null ? "Null domain object" : $"Wrong type of domain object: {nakedObjectAdapter.Object.GetType().FullName}"));
        }
    }
}