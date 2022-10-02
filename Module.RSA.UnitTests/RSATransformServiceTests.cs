﻿using System.Numerics;
using Module.RSA.Entities;
using Module.RSA.Entities.Abstract;
using Module.RSA.Services;
using Module.RSA.Services.Abstract;
using NUnit.Framework;

namespace Module.RSA.UnitTests;

[TestFixture]
public class RSATransformServiceTests
{
    private static readonly BigInteger E = 65537;

    private static readonly BigInteger D = BigInteger.Parse(
        "122934225883535365180355336751416971538799877297649721375496507050188816871686448825923864511376006422522909258008868068756122659174706950427245156656606435522649649610019770295533554113625689410568969317388619997812002011534383750029272612033156059263852009753864637086004638680615999136276564283354161765696487998035659593271338707508445200016959612240876802910702867700354861251610172315745344433378039412650812007845905743054340817614164968968219936304429411545679646258323246900826310170033287157534121845505708983390012868974090532259666544156143344308088692775495888475622365612873146955280085093068084254633419486495734895458498096095736582497133483331075416239481352366894234675247788911941315416682111470305433363307919622661758580318587684369046195220921308881663060829467067319434118176711957175336181159384995480433309538670084756644028610695593520150237353952986022304616608876106866304529411719800334329857195816398035882029646873029071911460339741555639055799692602428551143898069916760861665916015706186995700463622027604532288196972150381231836116175209617562399267809316383037453020884060510384392026906523512523759794728379842725004274576289232412950432364826792570461007531504012149276217414414948944507871903513"
    );

    private static readonly BigInteger N = BigInteger.Parse(
        "411289007184096034908619516293716578882961231224466271365864239243872810828511654331745995634389163965127566697745019481447241345363865915062043281015060287143186997115312965738852546630490928934629564661478278053836600940830512421546196292552807629586761405035429410419392822768468565796832773160354367228467544740786309804697724481800039464674637914514796213811768535426931264579446417022665916490392392107146677551595136284779842470977565448530819846116871220811129050836271919553803353392897623178483523578993703080526380795035732871136952693060450577156527064088400328910452957025415663485026848575445558108934158355007104183793113642877056880597834357201664278942457309642202386245066194646252778278938665118237364218076954448922419723153793631447799223142626691970623232448038504483621313141526290415211826269485237753478764750561420229845545021844637828755851415296966228551715092841676067708055431771412830835736499368413153193754713837875690491951273873109915294425270044561197967679915638445109076145846271325441268998330482081045445585598698690835949022299309685347963622621560541348971645867898673488399171169640888955627705668474770610958914835013862527430496545527046237020603902912275138311763813513047057462909834903"
    );

    private static readonly IRSAKey PublicKey = new RSAKey(E, N);
    private static readonly IRSAKey PrivateKey = new RSAKey(D, N);

    private IRSATransformService? _rsaTransformService;

    private Random? _random;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _rsaTransformService = new RSATransformService(new BigIntegerCalculationService());
    }

    [SetUp]
    public void SetUp()
    {
        _random = new Random(123);
    }

    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(256)]
    [TestCase(1000)]
    public void Transform_DefaultTest(int byteCount)
    {
        TestTransformWithRandom(byteCount);
    }

    [Test]
    [TestCase(10, 0)]
    [TestCase(10.5, 0)]
    [TestCase(1, -2)]
    [TestCase(1, -1)]
    [TestCase(1, 0)]
    [TestCase(1, 1)]
    [TestCase(1, 2)]
    public void Transform_NSizeBasedTest(double byteCountMultiplier, int byteCountShift)
    {
        var byteCount = (int)(N.GetByteCount(true) * byteCountMultiplier) + byteCountShift;
        TestTransformWithRandom(byteCount);
    }

    [Test]
    public void Transform_Test()
    {
        var dataValue = PublicKey.Modulus + 2;
        var data = dataValue.ToByteArray(true);

        TestTransform(data);
    }

    [Test]
    public void Transform_InvalidArgumentTest()
    {
        Assert.Throws<ArgumentException>(() =>
            _rsaTransformService!.Encrypt(Array.Empty<byte>(), PublicKey)
        );
        Assert.Throws<ArgumentException>(() =>
            _rsaTransformService!.Decrypt(Array.Empty<byte>(), PublicKey)
        );
    }

    private void TestTransformWithRandom(int byteCount)
    {
        var data = new byte[byteCount];
        _random!.NextBytes(data);

        TestTransform(data);
    }

    private void TestTransform(byte[] data)
    {
        var encrypted = _rsaTransformService!.Encrypt(data, PublicKey);
        Assert.That(encrypted, Is.Not.EqualTo(data));

        var decrypted = _rsaTransformService!.Decrypt(encrypted, PrivateKey);
        Assert.That(decrypted, Is.EqualTo(data));
    }
}