(function(e){function t(t){for(var r,a,i=t[0],c=t[1],l=t[2],d=0,u=[];d<i.length;d++)a=i[d],Object.prototype.hasOwnProperty.call(n,a)&&n[a]&&u.push(n[a][0]),n[a]=0;for(r in c)Object.prototype.hasOwnProperty.call(c,r)&&(e[r]=c[r]);p&&p(t);while(u.length)u.shift()();return o.push.apply(o,l||[]),s()}function s(){for(var e,t=0;t<o.length;t++){for(var s=o[t],r=!0,i=1;i<s.length;i++){var c=s[i];0!==n[c]&&(r=!1)}r&&(o.splice(t--,1),e=a(a.s=s[0]))}return e}var r={},n={app:0},o=[];function a(t){if(r[t])return r[t].exports;var s=r[t]={i:t,l:!1,exports:{}};return e[t].call(s.exports,s,s.exports,a),s.l=!0,s.exports}a.m=e,a.c=r,a.d=function(e,t,s){a.o(e,t)||Object.defineProperty(e,t,{enumerable:!0,get:s})},a.r=function(e){"undefined"!==typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(e,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(e,"__esModule",{value:!0})},a.t=function(e,t){if(1&t&&(e=a(e)),8&t)return e;if(4&t&&"object"===typeof e&&e&&e.__esModule)return e;var s=Object.create(null);if(a.r(s),Object.defineProperty(s,"default",{enumerable:!0,value:e}),2&t&&"string"!=typeof e)for(var r in e)a.d(s,r,function(t){return e[t]}.bind(null,r));return s},a.n=function(e){var t=e&&e.__esModule?function(){return e["default"]}:function(){return e};return a.d(t,"a",t),t},a.o=function(e,t){return Object.prototype.hasOwnProperty.call(e,t)},a.p="/";var i=window["webpackJsonp"]=window["webpackJsonp"]||[],c=i.push.bind(i);i.push=t,i=i.slice();for(var l=0;l<i.length;l++)t(i[l]);var p=c;o.push([0,"chunk-vendors"]),s()})({0:function(e,t,s){e.exports=s("56d7")},"0a25":function(e,t,s){},"217b":function(e,t,s){"use strict";var r=s("300d"),n=s.n(r);n.a},2764:function(e,t,s){e.exports=s.p+"img/success.2a0893b8.svg"},"2ee5":function(e,t,s){e.exports=s.p+"img/error.ffb4e352.svg"},"300d":function(e,t,s){},"38c2":function(e,t,s){},"56d7":function(e,t,s){"use strict";s.r(t);s("cadf"),s("551c"),s("f751"),s("097d");var r=s("2b0e"),n=function(){var e=this,t=e.$createElement,s=e._self._c||t;return s("div",{attrs:{id:"JsonRpcDocs"}},[s("ApiInfo",{attrs:{info:e.apiInfo.info}}),e._l(e.apiInfo.services,(function(e){return s("div",{key:e.path},[s("ApiService",{attrs:{service:e}})],1)}))],2)},o=[],a=function(){var e=this,t=e.$createElement,s=e._self._c||t;return s("div",{attrs:{id:"ApiInfo"}},[s("div",{staticClass:"api-title"},[e._v("\n    "+e._s(e.info.title)),s("small",[s("pre",{staticClass:"api-version"},[e._v(e._s(e.info.version))])])]),s("div",{staticClass:"api-endpoint"},[e._v("[ Endpoint: "+e._s(e.info.jsonRpcApiEndpoint)+" ]")]),s("div",{staticClass:"api-description"},[e._v(e._s(e.info.description))])])},i=[],c={name:"ApiInfo",props:{info:{description:String,version:String,title:String,contact:{name:String,email:String,url:String},jsonRpcApiEndpoint:String}}},l=c,p=(s("217b"),s("2877")),d=Object(p["a"])(l,a,i,!1,null,"5da0ddce",null),u=d.exports,h=function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("div",{attrs:{id:"ApiService"}},[r("button",{staticClass:"accordion",on:{click:e.toggleAccordion}},[r("div",{staticClass:"service-path"},[e._v(e._s(e.service.path))]),r("div",{staticClass:"service-name"},[e._v(e._s(e.service.name))]),r("div",{staticClass:"service-description"},[e._v(e._s(e.service.description))]),e.expanded?r("div",{staticStyle:{margin:"0 0 0 auto"}},[r("img",{staticClass:"service-arrow",attrs:{src:s("8c3e")}})]):r("div",{staticStyle:{margin:"0 0 0 auto"}},[r("img",{staticClass:"service-arrow",attrs:{src:s("f590")}})])]),r("div",{staticClass:"panel",style:{display:e.panelDisplay}},e._l(e.service.methods,(function(e){return r("div",{key:e.name},[r("ApiMethod",{staticClass:"service-method",attrs:{method:e}})],1)})),0)])},m=[],f=function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("div",{attrs:{id:"ApiMethod"}},[r("button",{staticClass:"accordion",style:{"border-radius":e.accordionBorderRadius},on:{click:e.toggleAccordion}},[r("div",{staticClass:"method-name"},[e._v(e._s(e.method.name))]),r("div",{staticClass:"method-description"},[e._v(e._s(e.method.description))])]),e.expanded?r("div",{staticClass:"panel"},[r("div",{staticClass:"method-subtitle"},[e._v("Parameters")]),r("div",{staticClass:"method-parameters"},[r("ApiMethodParameters",{attrs:{parameters:e.method.parameters},on:{parametersChanged:e.onParametersChanged}}),r("div",{staticClass:"call-method"},[e.showCallInProgressLoader?[e._m(0)]:[r("a",{staticClass:"call-method-button",attrs:{href:"#"},on:{click:e.callMethod}},[r("div",{staticClass:"face-primary"},[e._v("Try me!")]),r("div",{staticClass:"face-secondary"},[e._v("Go!")])])],e.websocketTriedToConnect?r("div",{staticClass:"call-method-status"},[e.websocketError?r("div",{staticClass:"call-method-error"},[r("img",{staticClass:"call-method-status-icon",attrs:{src:s("2ee5")}}),e._v("\n            "+e._s(e.websocketError)+"\n          ")]):r("div",{staticClass:"call-method-success"},[r("img",{staticClass:"call-method-status-icon",attrs:{src:s("2764")}})])]):e._e()],2)],1),e.hasResponse?r("div",{staticClass:"method-subtitle"},[e._v("Response")]):e._e(),r("div",{staticClass:"websocket-response"},[e.hasResponse?r("textarea",{directives:[{name:"model",rawName:"v-model",value:e.websocketResponse,expression:"websocketResponse"}],class:e.websocketResponseError?"websocket-response-error":"websocket-response-ok",attrs:{readonly:"",placeholder:"JSON response will show here",rows:e.websocketResponseRows},domProps:{value:e.websocketResponse},on:{input:function(t){t.target.composing||(e.websocketResponse=t.target.value)}}}):e._e()])]):e._e()])},v=[function(){var e=this,t=e.$createElement,s=e._self._c||t;return s("div",{staticClass:"call-method-loader-box"},[s("div",{staticClass:"call-method-loader"})])}],b=(s("28a5"),s("7f7f"),s("96cf"),s("3b8d")),g=function(){var e=this,t=e.$createElement,s=e._self._c||t;return s("div",{attrs:{id:"ApiMethodParameters"}},[s("textarea",{directives:[{name:"model",rawName:"v-model",value:e.parametersJson,expression:"parametersJson"}],class:e.jsonError?"method-parameters-code-error":"method-parameters-code-ok",attrs:{placeholder:"json parameters",rows:e.parametersCodeRows},domProps:{value:e.parametersJson},on:{change:e.emitParametersChange,input:function(t){t.target.composing||(e.parametersJson=t.target.value)}}}),e.jsonError?s("div",{staticClass:"method-parameters-error"},[e._v("\n    "+e._s(e.jsonError)+"\n  ")]):e._e()])},w=[],C=(s("ac6a"),{name:"ApiMethodParameters",data:function(){return{parametersJson:"",parametersCodeRows:1,jsonError:null}},props:{parameters:Array},mounted:function(){this.parametersJson=this.createParametersJsonTemplate(),this.emitParametersChange()},methods:{createParametersJsonTemplate:function(){var e={};return this.parameters.forEach((function(t){e[t.name]=t.type})),this.parametersCodeRows=this.parameters.length+2,JSON.stringify(e,null,2)},emitParametersChange:function(){var e;this.jsonError=null;try{e=JSON.parse(this.parametersJson)}catch(t){return void(this.jsonError="".concat(t.name,": ").concat(t.message))}this.$emit("parametersChanged",e)}}}),k=C,_=(s("5b12"),Object(p["a"])(k,g,w,!1,null,"5ddeae3c",null)),x=_.exports,y=s("2b7e"),R={name:"ApiMethod",components:{ApiMethodParameters:x},data:function(){return{expanded:!1,accordionBorderRadius:"5px",parametersJson:"",websocketError:null,websocketResponseOk:null,websocketResponseError:null,websocketTriedToConnect:!1,showCallInProgressLoader:!1,callInProgress:!1}},props:{method:{name:String,description:String,returns:String,parameters:Array}},methods:{toggleAccordion:function(){this.expanded=!this.expanded,this.accordionBorderRadius=this.expanded?"5px 5px 0px 0px":"5px"},onParametersChanged:function(e){this.parametersJson=e},callMethod:function(){var e=Object(b["a"])(regeneratorRuntime.mark((function e(){var t,s,r=this;return regeneratorRuntime.wrap((function(e){while(1)switch(e.prev=e.next){case 0:if(!this.callInProgress){e.next=2;break}return e.abrupt("return");case 2:return this.callInProgress=!0,t=setTimeout((function(){r.showCallInProgressLoader=!0}),1e3),this.websocketError=null,this.websocketResponseOk=null,this.websocketResponseError=null,this.websocketTriedToConnect=!1,s=new y["JsonRpcWebsocket"]("ws://localhost:54624/chat",2e3),e.prev=9,e.next=12,s.open();case 12:e.next=17;break;case 14:e.prev=14,e.t0=e["catch"](9),this.websocketError="Failed to establish connection";case 17:if(this.websocketTriedToConnect=!0,s.state===y["WebsocketReadyStates"].OPEN){e.next=22;break}return clearTimeout(t),this.callInProgress=this.showCallInProgressLoader=!1,e.abrupt("return");case 22:this.hasResponse?s.call(this.method.name,this.parametersJson).then((function(e){r.websocketResponseOk=JSON.stringify(e,null,2),s.close(),clearTimeout(t),r.callInProgress=r.showCallInProgressLoader=!1})).catch((function(e){r.websocketResponseError=JSON.stringify(e,null,2),s.close(),clearTimeout(t),r.callInProgress=r.showCallInProgressLoader=!1})):(s.notify(this.method.name,this.parametersJson),clearTimeout(t),this.callInProgress=this.showCallInProgressLoader=!1,s.close());case 23:case"end":return e.stop()}}),e,this,[[9,14]])})));function t(){return e.apply(this,arguments)}return t}()},computed:{hasResponse:function(){return"void"!==this.method.returns.toLowerCase()},websocketResponse:function(){var e=this.websocketResponseError?this.websocketResponseError:this.websocketResponseOk;return e||""},websocketResponseRows:function(){return this.websocketResponse?this.websocketResponse.split("\n").length:1}}},P=R,S=(s("7108"),Object(p["a"])(P,f,v,!1,null,"1b7f0b14",null)),E=S.exports,O={name:"ApiService",components:{ApiMethod:E},data:function(){return{expanded:!1,panelDisplay:"none"}},props:{service:{name:String,path:String,description:String,methods:[]}},methods:{toggleAccordion:function(){this.expanded=!this.expanded,this.panelDisplay=this.expanded?"block":"none"}}},j=O,A=(s("8b75"),Object(p["a"])(j,h,m,!1,null,"51731785",null)),J=A.exports,T={name:"JsonRpcDocs",components:{ApiInfo:u,ApiService:J},data:function(){return{apiInfo:{info:{},services:[]}}},methods:{readTextFile:function(e,t){var s=new XMLHttpRequest;s.overrideMimeType("application/json"),s.open("GET",e,!0),s.onreadystatechange=function(){4===s.readyState&&"200"==s.status&&t(s.responseText)},s.send(null)}},mounted:function(){var e=this;this.readTextFile("./jsonRpcApi.json",(function(t){e.apiInfo=JSON.parse(t)}))}},I=T,M=(s("d441"),Object(p["a"])(I,n,o,!1,null,null,null)),L=M.exports;r["a"].config.productionTip=!1,new r["a"]({render:function(e){return e(L)}}).$mount("#JsonRpcDoc")},"5b12":function(e,t,s){"use strict";var r=s("0a25"),n=s.n(r);n.a},"6f8b":function(e,t,s){},7108:function(e,t,s){"use strict";var r=s("38c2"),n=s.n(r);n.a},"77ff":function(e,t,s){},"8b75":function(e,t,s){"use strict";var r=s("77ff"),n=s.n(r);n.a},"8c3e":function(e,t,s){e.exports=s.p+"img/down-arrow.9aef94ce.svg"},d441:function(e,t,s){"use strict";var r=s("6f8b"),n=s.n(r);n.a},f590:function(e,t,s){e.exports=s.p+"img/right-arrow.be237328.svg"}});