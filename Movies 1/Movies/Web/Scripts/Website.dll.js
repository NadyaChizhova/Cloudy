(function()
{
 var Global=this,Runtime=this.IntelliFactory.Runtime,Website,Forms,WebSharper,Formlet,Enhance,FormContainerConfiguration,Remoting,List,Html,Default,Formlet1,Controls,Data,FormButtonConfiguration,window,Operators,Control,_FSharpEvent_1,Formlet2,Base,_Result_1,Concurrency;
 Runtime.Define(Global,{
  Website:{
   AddMovieControl:Runtime.Class({
    get_Body:function()
    {
     return Forms.AddMovie();
    }
   }),
   Forms:{
    AddMovie:function()
    {
     var textForm,x,x1,f,f1,fc,inputRecord,Description,x2,f2,f3,x3,f4,f5,proc,x6,_builder_,f7;
     textForm=(x=(x1=Forms.MovieTitleForm(),(f=function(formlet)
     {
      return Enhance.WithSubmitButton(formlet);
     },f(x1))),(f1=(fc=(inputRecord=FormContainerConfiguration.get_Default(),(Description=(x2=(f2=function(arg0)
     {
      return{
       $:0,
       $0:arg0
      };
     },f2("Please enter movie title.")),(f3=function(arg0)
     {
      return{
       $:1,
       $0:arg0
      };
     },f3(x2))),Runtime.New(FormContainerConfiguration,{
      Header:(x3=(f4=function(arg0)
      {
       return{
        $:0,
        $0:arg0
       };
      },f4("Type Movie title to add")),(f5=function(arg0)
      {
       return{
        $:1,
        $0:arg0
       };
      },f5(x3))),
      Padding:inputRecord.Padding,
      Description:Description,
      BackgroundColor:inputRecord.BackgroundColor,
      BorderColor:inputRecord.BorderColor,
      CssClass:inputRecord.CssClass,
      Style:inputRecord.Style
     }))),function(formlet)
     {
      return Enhance.WithCustomFormContainer(fc,formlet);
     }),f1(x)));
     proc=function(title)
     {
      return function()
      {
       var value,x4,f6,_this;
       value=Remoting.Call("Website:1",[title]);
       value;
       x4=List.ofArray([Default.P(List.ofArray([(f6=function(x5)
       {
        return Default.Text(x5);
       },f6("Title is succesfully added"))]))]);
       _this=Default.Tags();
       return _this.NewTag("fieldset",x4);
      };
     };
     x6=(_builder_=Formlet1.Do(),_builder_.Delay(function()
     {
      return _builder_.Bind(textForm,function(_arg1)
      {
       return _builder_.ReturnFrom(Formlet1.OfElement(proc(_arg1)));
      });
     }));
     f7=function(formlet)
     {
      return Formlet1.Flowlet(formlet);
     };
     return f7(x6);
    },
    Input:function(label,err)
    {
     var x,x1,x2,f,f1,f2;
     x=(x1=(x2=Controls.Input(""),(f=function(arg10)
     {
      return Data.Validator().IsNotEmpty(err,arg10);
     },f(x2))),(f1=function(formlet)
     {
      return Enhance.WithValidationIcon(formlet);
     },f1(x1)));
     f2=function(formlet)
     {
      return Enhance.WithTextLabel(label,formlet);
     };
     return f2(x);
    },
    InputInt:function(label,err)
    {
     var x,x1,x2,x3,f,f1,f2,f3,f4;
     x=(x1=(x2=(x3=Controls.Input(""),(f=Data.Validator().IsInt(err),f(x3))),(f1=function(formlet)
     {
      return Enhance.WithValidationIcon(formlet);
     },f1(x2))),(f2=function(formlet)
     {
      return Enhance.WithTextLabel(label,formlet);
     },f2(x1)));
     f3=(f4=function(value)
     {
      return value<<0;
     },function(formlet)
     {
      return Formlet1.Map(f4,formlet);
     });
     return f3(x);
    },
    LoginForm:function(redirectUrl)
    {
     var uName,x,x1,x2,f,f1,f2,pw,x3,x4,x5,f3,f4,f5,loginF,x6,x7,_builder_,f8;
     uName=(x=(x1=(x2=Controls.Input(""),(f=function(arg10)
     {
      return Data.Validator().IsNotEmpty("Enter Username",arg10);
     },f(x2))),(f1=function(formlet)
     {
      return Enhance.WithValidationIcon(formlet);
     },f1(x1))),(f2=function(formlet)
     {
      return Enhance.WithTextLabel("Username",formlet);
     },f2(x)));
     pw=(x3=(x4=(x5=Controls.Password(""),(f3=function(arg10)
     {
      return Data.Validator().IsNotEmpty("Enter Password",arg10);
     },f3(x5))),(f4=function(formlet)
     {
      return Enhance.WithValidationIcon(formlet);
     },f4(x4))),(f5=function(formlet)
     {
      return Enhance.WithTextLabel("Password",formlet);
     },f5(x3)));
     loginF=Data.$(Data.$((x6=function(n)
     {
      return function(pw1)
      {
       return{
        Name:n,
        Password:pw1
       };
      };
     },Formlet1.Return(x6)),uName),pw);
     x7=(_builder_=Formlet1.Do(),_builder_.Delay(function()
     {
      var f6,submitConf,inputRecord,resetConf,inputRecord1;
      return _builder_.Bind((f6=(submitConf=(inputRecord=FormButtonConfiguration.get_Default(),Runtime.New(FormButtonConfiguration,{
       Label:{
        $:1,
        $0:"Login"
       },
       Style:inputRecord.Style,
       Class:inputRecord.Class
      })),(resetConf=(inputRecord1=FormButtonConfiguration.get_Default(),Runtime.New(FormButtonConfiguration,{
       Label:{
        $:1,
        $0:"Reset"
       },
       Style:inputRecord1.Style,
       Class:inputRecord1.Class
      })),function(formlet)
      {
       return Enhance.WithCustomSubmitAndResetButtons(submitConf,resetConf,formlet);
      })),f6(loginF)),function(_arg4)
      {
       var a,f7;
       return _builder_.ReturnFrom((a=Remoting.Async("Website:0",[_arg4]),(f7=function(loggedIn)
       {
        var _;
        if(loggedIn)
         {
          _=window;
          _.location=redirectUrl;
          redirectUrl;
          return Formlet1.Return(null);
         }
        else
         {
          return Forms.WarningPanel("Login failed");
         }
       },Forms.WithLoadingPane(a,f7))));
      });
     }));
     f8=function(formlet)
     {
      return Enhance.WithFormContainer(formlet);
     };
     return f8(x7);
    },
    MovieTitleForm:function()
    {
     var x;
     return Data.$((x=function(name)
     {
      return name;
     },Formlet1.Return(x)),Forms.Input("Title","Please enter a title"));
    },
    WarningPanel:function(label)
    {
     var _builder_;
     _builder_=Formlet1.Do();
     return _builder_.Delay(function()
     {
      var genElem;
      return _builder_.Bind((genElem=function()
      {
       return Operators.add(Default.Div(List.ofArray([Default.Attr().Class("warningPanel")])),List.ofArray([Default.Text(label)]));
      },Formlet1.OfElement(genElem)),function()
      {
       return _builder_.ReturnFrom(Formlet1.Never());
      });
     });
    },
    WithLoadingPane:function(a,f)
    {
     var loadingPane,f1;
     loadingPane=(f1=function()
     {
      var elem,state,x,f2,f4;
      elem=Default.Div(List.ofArray([Default.Attr().Class("loadingPane")]));
      state=_FSharpEvent_1.New();
      x=(f2=function()
      {
       var f3;
       f3=function(_arg3)
       {
        var x1;
        x1=Runtime.New(_Result_1,{
         $:0,
         $0:_arg3
        });
        state.event.Trigger(x1);
        return Concurrency.Return(null);
       };
       return Concurrency.Bind(a,f3);
      },Concurrency.Delay(f2));
      f4=function(arg00)
      {
       var t;
       t={
        $:0
       };
       return Concurrency.Start(arg00);
      };
      f4(x);
      return[elem,function(value)
      {
       value;
      },state.event];
     },Formlet1.BuildFormlet(f1));
     return Formlet1.Replace(loadingPane,f);
    }
   },
   LoginControl:Runtime.Class({
    get_Body:function()
    {
     return Forms.LoginForm(this.redirectUrl);
    }
   })
  }
 });
 Runtime.OnInit(function()
 {
  Website=Runtime.Safe(Global.Website);
  Forms=Runtime.Safe(Website.Forms);
  WebSharper=Runtime.Safe(Global.IntelliFactory.WebSharper);
  Formlet=Runtime.Safe(WebSharper.Formlet);
  Enhance=Runtime.Safe(Formlet.Enhance);
  FormContainerConfiguration=Runtime.Safe(Enhance.FormContainerConfiguration);
  Remoting=Runtime.Safe(WebSharper.Remoting);
  List=Runtime.Safe(WebSharper.List);
  Html=Runtime.Safe(WebSharper.Html);
  Default=Runtime.Safe(Html.Default);
  Formlet1=Runtime.Safe(Formlet.Formlet);
  Controls=Runtime.Safe(Formlet.Controls);
  Data=Runtime.Safe(Formlet.Data);
  FormButtonConfiguration=Runtime.Safe(Enhance.FormButtonConfiguration);
  window=Runtime.Safe(Global.window);
  Operators=Runtime.Safe(Html.Operators);
  Control=Runtime.Safe(WebSharper.Control);
  _FSharpEvent_1=Runtime.Safe(Control["FSharpEvent`1"]);
  Formlet2=Runtime.Safe(Global.IntelliFactory.Formlet);
  Base=Runtime.Safe(Formlet2.Base);
  _Result_1=Runtime.Safe(Base["Result`1"]);
  return Concurrency=Runtime.Safe(WebSharper.Concurrency);
 });
 Runtime.OnLoad(function()
 {
 });
}());
