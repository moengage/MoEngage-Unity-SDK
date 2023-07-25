<<<<<<< HEAD
﻿/*
=======
/*
>>>>>>> MOEN-20006-Unity-Multiinstance
 * Copyright (c) 2014-2020 MoEngage Inc.
 *
 * All rights reserved.
 *
 *  Use of source code or binaries contained within MoEngage SDK is permitted only to enable use of the MoEngage platform by customers of MoEngage.
 *  Modification of source code and inclusion in mobile apps is explicitly allowed provided that all other conditions are met.
 *  Neither the name of MoEngage nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
 *  Redistribution of source code or binaries is disallowed except with specific prior written permission. Any such redistribution must retain the above copyright notice, this list of conditions and the following disclaimer.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
namespace MoEngage
{
  [System.Serializable]
  public class SelfHandled
  {
    public string payload;
    public long dismissInterval;
    public bool isCancellable;

    public SelfHandled(Dictionary < string, object > selfHandledDictionary) {
      if (selfHandledDictionary.ContainsKey(MoEConstants.PARAM_PAYLOAD)) {
        payload = selfHandledDictionary[MoEConstants.PARAM_PAYLOAD] as string;
      };

      if (selfHandledDictionary.ContainsKey(MoEConstants.PARAM_DISMISS_INTERVAL)) {
        dismissInterval = (long) selfHandledDictionary[MoEConstants.PARAM_DISMISS_INTERVAL];
      }

      if (selfHandledDictionary.ContainsKey(MoEConstants.PARAM_IS_CANCELLABLE)) {
        isCancellable = (bool) selfHandledDictionary[MoEConstants.PARAM_IS_CANCELLABLE];
      }
    }
  }
}
=======
namespace MoEngage {
  [System.Serializable]
<<<<<<<< HEAD:Core-SDK/Assets/MoEngage/Scripts/Model/AccountMeta.cs
  public class AccountMeta {
    public string appId;

    public AccountMeta(string appId) {
      this.appId = appId;
    }
========
  /// <summary>
  /// SelfHandled Payload information
  /// </summary>
  public class SelfHandled {
    /// <value> Self handled campaign payload. </value>
    public string payload;

    /// <value>  Interval after which in-app should be dismissed, unit - Seconds </value>
    public long dismissInterval;

    /// <value>Should the campaign be dismissed by pressing the back button or using the back gesture. if the value is true campaign should be dismissed on back press.  </value>
    public bool isCancellable;
>>>>>>>> MOEN-20006-Unity-Multiinstance:Core-SDK/Assets/MoEngage/Scripts/Model/SelfHandled.cs
  }
}
>>>>>>> MOEN-20006-Unity-Multiinstance
