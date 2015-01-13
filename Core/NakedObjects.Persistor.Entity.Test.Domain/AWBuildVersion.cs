// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.ComponentModel.DataAnnotations;
using NakedObjects;

namespace AdventureWorksModel {
    // ReSharper disable once PartialTypeWithSinglePart

    public partial class AWBuildVersion {
        #region Primitive Properties

        #region SystemInformationID (Byte)

        [MemberOrder(100)]
        public virtual byte SystemInformationID { get; set; }

        #endregion

        #region Database_Version (String)

        [MemberOrder(110), StringLength(25)]
        public virtual string Database_Version { get; set; }

        #endregion

        #region VersionDate (DateTime)

        [MemberOrder(120), Mask("d")]
        public virtual DateTime VersionDate { get; set; }

        #endregion

        #region ModifiedDate (DateTime)

        [MemberOrder(130), Mask("d")]
        public virtual DateTime ModifiedDate { get; set; }

        #endregion

        #endregion
    }
}