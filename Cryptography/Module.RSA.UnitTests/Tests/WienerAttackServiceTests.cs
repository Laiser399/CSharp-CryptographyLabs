﻿using System.Numerics;
using Autofac;
using Module.RSA.Exceptions;
using Module.RSA.Services.Abstract;
using Module.RSA.UnitTests.Modules;
using NUnit.Framework;

namespace Module.RSA.UnitTests.Tests;

[TestFixture]
public class WienerAttackServiceTests
{
    private readonly IContainer _container;

    private IRSAAttackService? _rsaAttackService;

    public WienerAttackServiceTests()
    {
        _container = BuildContainer();
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _rsaAttackService = _container.Resolve<IRSAAttackService>();
    }

    [Test]
    // p x q: 4bytes x 4bytes
    // p = 4014345277
    // q = 2261358851
    [TestCase(
        "9077875223113996727",
        "7331855134460316091",
        "3811"
    )]
    // p x q: 16bytes x 16bytes
    // p = 266983238706185107328323683012992961689
    // q = 176366836242169232790292053698844346087
    [TestCase(
        "47086989140297727095161114268686052844365725307730262084840693753282648060943",
        "20887563742003832508895787020488352413888027220515674017017932856138092347691",
        "3431875928858634707"
    )]
    // p x q: 256bytes x 256bytes
    // p = 30712012015087877934692664304331193166402591067530993643671434606448897213400282251515466152062371623805736951046350512168466029167032295000713887478774189599112893772240456386060940172091066322758920355818132346113664525258054408461041846085748379762286627858280926205532116116502167311837242361604380008932303033724207557737561217100687950626295781325664881359448282379122757860718289087612343819571178646571654786289213397981883325316753926403464881011039704002133915355602106442576623156926706635729281181425785436339027848383622743490227088140606857570097884353369323973493241739467474701027775718394788193743519
    // q = 17852362528729523468617438537008329947830601870588367719052470648767928486473491808524557196006483460945668549225145796875312380537627357358822818733813238522773590004809809405748706333687679513408389384010268030915616193264853168835553345048081776991116711781876633201478288639284234898997983045683451888428708705095933822700197119182499746871056237460537936422161771267024069176112510185475010641836726761699642818351138616928198172577407242133796699139202687710644188090195649499292728344698511536231022436450886923079086940599561916672915400102692789943347577810197454518150416380729805574695772853834687122884641
    [TestCase(
        "548281972480045736201812708225981161699160504840930997833445812650701729309420263887568767462829044213229354453180882041427497825609385369591584439990214262038373878872263038393975836380912569634140889609113366454172080491764980436292005398055813902066650130095197099872916005681709422262576044478647285843899253825866880001332994017378115497053309229482830031008831688283477865418281882301312686076444111202620683222150967717695197447318129618185349506097190019678821976457208550346356719412191280390363544380588964195780668076840096531626503359416858261149905294348144968278352599191869910397977957790621103672835536250663142840823416787288861980204762371562438083810826814704999452311518476163387210487471866137129862590442139738736660546148547141410068061566492605536188961536124444088429256256866358280136469125108822720306895713892633521297148498407253555608961243978954202872291371206800508079402202472078370001627527889697640513442928644721449514370874195197963467259248881814617165437815392133246378386465204244036933555374145488264618765503439894068980354817702772449377964009944587142170120852093914435340285318137044178464431419293241270180652183453193125165714527805642795485684213069229297906891721421547102763778391679",
        "99664801693788558507786590579838616761265266269872967947784895498460786494155848985523947274225181663563454419584615581097833671497773577065449208919965016815717216798235544379107531027210631646294684924343506631161592571379042097422924011509818672883195837122878850405258151367911165952764754958239572150159525243197315266505675813045990234178250537810438335107941326701540298676316795985558796332917840230795311346603325054798050186654408627970047729334775655727798741490040014433478339453992907715341754238251435705645051331305018612624029928640286713602397138488583252204939098872748953598802881748047055987538035535930402024378539605677787872926963258859256915780982951002138266340051034911765974610913864066668492866231003608561932015580359014646148314093437080156900328450117675421784549191015061331194540492768305536701821091486706804330674312786762616138863260749149868269589538372397457207162987303184694447470159752270032270326413374909096097411414649476866440976598601326394818646989818813343183427469155015466617633862344040261124566226838645016448787935411844938280840611859562545873920100074613291228001027562571265397414061075153691242888081141217718371666049448148461119629204781337526336249992693466337911219598141",
        "48743426087441691559165071399633475287073085013853844919191121976259098345972141058955851483996498810493913697603676442391261068312476780250269036943918438467301470927673156845409773892849709060020561321755066586043292887820553359378478613821381879164496505636761578301916060102230222103438193565186171473941",
        Ignore = "Very long duration"
    )]
    public async Task Attack_TestAsync(
        string modulusStr,
        string publicExponentStr,
        string expectedPrivateExponentStr)
    {
        var modulus = BigInteger.Parse(modulusStr);
        var publicExponent = BigInteger.Parse(publicExponentStr);
        var expectedPrivateExponent = BigInteger.Parse(expectedPrivateExponentStr);

        var actualPrivateExponent = await _rsaAttackService!.AttackAsync(publicExponent, modulus);

        Assert.AreEqual(expectedPrivateExponent, actualPrivateExponent);
    }

    [Test]
    [TestCase("2118100319", "65537")]
    [TestCase("111309534155653", "65537")]
    public void Attack_WithoutVulnerabilityTest(
        string modulusStr,
        string publicExponentStr)
    {
        var modulus = BigInteger.Parse(modulusStr);
        var publicExponent = BigInteger.Parse(publicExponentStr);

        Assert.ThrowsAsync<CryptographyAttackException>(
            () => _rsaAttackService!.AttackAsync(publicExponent, modulus)
        );
    }

    private static IContainer BuildContainer()
    {
        var builder = new ContainerBuilder();

        builder.RegisterModule<WienerAttackModule>();

        return builder.Build();
    }
}