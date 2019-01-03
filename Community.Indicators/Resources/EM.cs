using WealthLab.Extensions.Attribute;

// To satisfy for the Extension Manager
[assembly: ExtensionInfo(
    ExtensionType.Indicator,
    "Community.Indicators",
    "Community Indicators library",
    "A collection of technical indicators for the Wealth-Lab community",
    "2019.01",
    "MS123",
    "Community.Indicators.CommunityIndicators.gif",
    ExtensionLicence.Freeware,
    new string[] { "WealthLab.Indicators.Community.dll" },
    MinProVersion = "6.9",
    MinDeveloperVersion = "6.9",
    PublisherUrl = "http://www2.wealth-lab.com/WL5Wiki/CommunityIndicatorsMain.ashx")
    ]

namespace Community.Indicators.Resources
{
    class EM
    {
    }
}
