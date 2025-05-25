# Cezzi.Applications
General application functionality and extensions. 
<br/>

|  |  |  
|---|---|
| **Source Code** | [$/Cezzi/src/Cezzi.Applications](https://dev.azure.com/mtnvencenzo/_git/Cezzi/_git/Cezzi?path=/Cezzi/src/Cezzi.Applications) | 
| **Nuget** | [![Azure Artifacts](https://feeds.dev.azure.com/mtnvencenzo/_apis/public/Packaging/Feeds/Global/Packages/5cb13dce-fb89-45e0-8688-7d1993d0f192/Badge)](https://dev.azure.com/mtnvencenzo/Cezzi/_artifacts/feed/Global/NuGet/Cezzi.Applications?preferRelease=true) |

<br/>

# Extension Methods
This package contains numerous extension methods for common string, numeric and other .Net datatypes.  Below is a inventory of the various extension classes.

> All extension classes are in the `Cezzi.Applications.Extensions` namespace.

|  |
|:---|
| [DataTableExtensions](./Extensions/DataTableExtensions.cs) |
| [DateTimeExtensions](./Extensions/DateTimeExtensions.cs) |
| [EnumExtensions](./Extensions/EnumExtensions.cs) |
| [IListExtensions](./Extensions/IListExtensions.cs) |
| [IntExtensions](./Extensions/IntExtensions.cs) |
| [LongExtensions](./Extensions/LongExtensions.cs) |
| [ObjectExtensions](./Extensions/ObjectExtensions.cs) |
| [ReferenceTypeExtensions](./Extensions/ReferenceTypeExtensions.cs) |
| [ReflectionExtensions](./Extensions/ReflectionExtensions.cs) |
| [StringExtensions](./Extensions/StringExtensions.cs) |
| [TaskExtensions](./Extensions/TaskExtensions.cs) |
| [XmlExtensions](./Extensions/XmlExtensions.cs) |


# Logging Monikers
This package contains a number of logging monikers for use with Structured Logging implmentations.

> All logging moniker classes are in the `Cezzi.Applications.Logging` namespace.

|  |
|:---|
| [ApiMonikers](./Logging/ApiMonikers.cs) |
| [AppMonikers](./Logging/AppMonikers.cs) |
| [AzMonikers](./Logging/AzMonikers.cs) |
| [ServiceBusMonikers](./Logging/ServiceBusMonikers.cs) |

# Input Guards
Use the [Guard](./Guard.cs) class for asserting via exceptions that provided input is in an expected state.  This is not meant as a validation library 
but more for asserting that application expectations of input are met.  

***Note that each method on the `Guard` class rasies an exception.***

``` csharp
public void MyMethod(object input)
{
     Guard.NotNull(input, nameof(input));
}
```

# StopWatch
Use the [StopWatch](./StopWatch.cs) class for basic clock timing of code execution in milliseconds.
``` csharp
public void MyMethod()
{
    var stopWatch = new StopWatch();

    // Do long running operation here
    
    var milliseconds = stopWatch.Elapsed();
}
```

# Email Validation
Use the [EmailValidator](./Validators/EmailValidator.cs) class to validate email addresses.

``` csharp
if (EmailValidator.Validate("machoman+savage@not-really-gmail.com"))
{
    this.OhYea();

    //####(((////*********,,,*,*,,,,,*,,,,*,,,,*,*,*********************//*/*/**////*//***////////////////
    //,,,,,,,,,,,,,,,,,,*,,*****************,,*,,,,,*,*****************************/****/////*////////////
    //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,**,*,,,,**,,,,,,,,,***************************************///////////
    //,,,,,,,,,,,,,,,,,,(#(,,,,,,,*,,,,,,*,,,,,,,,,,,,,,,,*********************************/*******///////
    //,,,,,,,,,,,,,,(%#&#&#/#(,,,,,,,,,,,,,,,,,,,,,,,,,**,,,,,*,********************(//*/***************//
    //,.......,,.%&(&%#&%%%(%#,,,,,,,,,,,,,,,,,,,/,//(#(((#/*(,,,,,*,*************%&%*%/&/,***************
    //..........&&%%#&&&&&@@&/,,,,,,,,,,,,,,,,,.%*%(%#((//*(*/*,,,,,*,*,*********/@&#(#/#///,,/(,*********
    //........*%%((((%*.,,,.,,,,,,,,,,,,,,,,,,,*,(#&&&@&@&&#*//*,,,,,,,**,********%%&&&%%%*&&%%(*,,*******
    //......(%##(/(%#.........,...,,,,,,,,,,,,.&&@@@@@@.@@@&&&*#,,,,,,,,,,*,****************/&%(///*******
    //....(%##(//(((............,..,,,,,,,,,,/#%&.@@@&%(*#@&% &@@#/,,,,,,,,,*,,**************(#&%(/,,*****
    //.. #%#((*/(/(,.................,,,,,,,**#&%/%@@@%(//*&(/&@&&#//,,,,,,,,,,,,,*,**********/#&%(*,,/***
    //. *%##((//(((...................,...,,##%&#@&&@&#(**/%#%%@@&%##*,,,,,,,,,,,*,,,**,*******##&///,,***
    // /&%%##(((((   (###((/.............,,*(/&@@&@@@&&&%&&@&&@@@&##((,,,,,,,,,,,,,,,,,,*,****/%%&#(/,,./*
    // &&%##/((((#((%%%##((((((##%###(((&@***/#%#%@@@@@@@@&#((@&%##(,&@@@@/****/(,*#((((///#,,#%&%%#/*, **
    //%&%####(((((((((#####((/////@&%#(@*@@@*(&@%%@@@&##%(/(((&@&@@&@@@@@/@&,,,/(((((//****/(#%(&%/%/(*,**
    //  #&%###((#((((((((((##%##(/@@&#@@@%@@@@@(@&#@&&.,,,./%@&##@@@@@@@&&,@,/%####((((////((((#(#(/*(/*,*
    //     &&%%%%##((#(((((/((#%%%@@@@@/&@/@@@(,(*%&(%,.*@@(%.(,&@@@@@(%%/,@###(((/(((((/(((#(((#(((/**/(*
    //         %&&%%%%##(((((##%&@@@@@@%@@%@@@@@&@#/%,/.,/*,#@@@@@@@#*@(/*/&@#%%%&&%###((#////((##%%,*****
    //              (%%%%####%%&@@@@@@@@@@@@@@@%.@(%%%.(%@*@ @@@@@#%&#@/#//(#//(%&&&&&&&&#%%%%%,*,,,,,,,**
    //                  ........,,%@@@@%@&@@@@@@@.@#&.,#*%,@@@@@@@%#@#@*&/*(%,*/,,,,,,,,,,,,*,,,,,,,,*,***
    //                   . ......*#&@@@@@@@@@@@@@@@@&%##,@@&(*@@@@&#&%@/@&//@//,,,,,,,,,,,,,,,,,,,,,***,**
    //                      .....**#@@@@@@@@@@@@@@@@@@( %@@&@(@@@@@*@@@%&@/*///,,,,,,,,,,,,,,,,,,,,,,,,,**
    //                  .  ......./(&%%@&@@@@@@@@@@&@@@&@@@@@/@@@@@#@@@@@#**/(,,,,,,,,,,,,,,,,,,,,,,,,,*,*
    //                     ........***&%%@&@@@@@@@@&%@/@@@@@@@@@&@@#@@%@,@*/(,,,,,,,,,,,,,,,,,,,,,,,,,,,,*
    //                     . ......./%@%@@(&@@&@@@@@/@.@*@@@@&@@@@@/@@%**%//,,,,,,,,,,,,,,,,,,,,,,,,,,,,,*
    //                    . .........#&(/&/&@@&@@@@@@&..@@@@@%@&@&@@@ &%@#(,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,*
    //       ... .....................%&.&,*%&@@%@@@@...@%@@@@@@&,&%     *,,,,,,,,,,,,,,,,,,,,,,,,,,,,,***
}
```

