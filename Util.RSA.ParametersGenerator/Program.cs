using Autofac;
using Util.RSA.ParametersGenerator;
using Util.RSA.ParametersGenerator.Services.Abstract;

var container = AppContainer.Build();

var rsaParametersGenerator = container.Resolve<IRsaParametersGenerator>();

rsaParametersGenerator.GenerateAndSave();