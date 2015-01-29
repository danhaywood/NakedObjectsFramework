// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System.ComponentModel.DataAnnotations.Schema;
using NakedObjects;
using NakedObjects.Services;

namespace ModelFirst {
    [ComplexType]
    public class NameType : AbstractTestCode {
        [Root]
        public Person Parent { get; set; }

        public IDomainObjectContainer Container { protected get; set; }

        public SimpleRepository<Person> Service { protected get; set; }

        #region Primitive Properties

        public string Firstname { get; set; }
        public string Surname { get; set; }

        #endregion

        [NakedObjectsIgnore]
        public object ExposeContainerForTest() {
            return Container;
        }

        [NakedObjectsIgnore]
        public object ExposeServiceForTest() {
            return Service;
        }
    }
}