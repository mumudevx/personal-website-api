using Microsoft.Extensions.Localization;

namespace Web.Api.Constants.Messages;

public class PropertyValidationMessages(IStringLocalizer<PropertyValidationMessages> localize)
{
    // General
    public string TitleRequired => localize["TitleRequired"];
    public string TitleMaxLength => localize["TitleMaxLength"];
    public string ShortContentRequired => localize["ShortContentRequired"];
    public string ShortContentMaxLength => localize["ShortContentMaxLength"];
    public string SlugRequired => localize["SlugRequired"];
    public string ContentRequired => localize["ContentRequired"];
    public string FeaturedImageRequired => localize["FeaturedImageRequired"];
    public string NameRequired => localize["NameRequired"];
    public string AltRequired => localize["AltRequired"];
    public string PathRequired => localize["PathRequired"];

    // Book
    public string AuthorRequired => localize["AuthorRequired"];
    public string PagesRequired => localize["PagesRequired"];
    public string StartReadingDateRequired => localize["StartReadingDateRequired"];
    public string EndReadingDateRequired => localize["EndReadingDateRequired"];

    // HealthActivity
    public string ActivityTypeRequired => localize["ActivityTypeRequired"];
    public string CaloriesRequired => localize["CaloriesRequired"];
    public string ActionStartDateRequired => localize["ActionStartDateRequired"];
    public string ActionEndDateRequired => localize["ActionEndDateRequired"];

    // PhotoGallery
    public string OrderRequired => localize["OrderRequired"];
    public string PhotoGalleryIdRequired => localize["PhotoGalleryIdRequired"];
    public string ImageFileIdRequired => localize["ImageFileIdRequired"];
}