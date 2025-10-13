using EPiServer.Cms.TinyMce.Core;

namespace ChildFund.Infrastructure.Initialization;

public static class InitializeTinyMce
{
	public static IServiceCollection AddTinyMceConfiguration(this IServiceCollection services)
	{
		services.Configure<TinyMceConfiguration>(config =>
		{
			//Text Styles
			var textStyles = new[]
			{
				new { title = "Small Text", block = "small" },
				new { title = "Blockquote", block = "blockquote" }
			};

			// Templates
			var templates = new[]
			{
				new
				{
					title = "Quote template",
					description = "Block quote",
					url = "/html/templates/blockquote.html"
				},
				new
				{
					title = "Image with description template",
					description = "Image",
					url = "/html/templates/image.html"
				}
			};

			var line1 = "undo redo | blocks styles | bold italic underline subscript superscript charmap";
			var line2 = "alignleft aligncenter alignright alignjustify | numlist bullist indent outdent | cut copy pastetext";
			var line3 = "table | epi-link anchor | image epi-image-editor media | epi-personalized-content | preview | removeformat code fullscreen";

			config.Default()
				.AddEpiserverSupport()
				.ContentCss("/css/rtf_editor.css")
				.AddPlugin("table")
				.Menubar("edit table")
				.AddPlugin("epi-link code epi-personalized-content media wordcount anchor preview charmap")
				.Toolbar(line1, line2, line3)
				.StyleFormats(textStyles)
				.BlockFormats("Paragraph=p;Heading 2=h2;Heading 3=h3;Heading 4=h4;Heading 5=h5;Heading 6=h6")
				.AddSetting("templates", templates)
				.AddSetting("image_caption", true)
				.AddSetting("image_advtab", true)
				.AddSetting("extended_valid_elements", "i[class], span")
				.AddSetting("force_br_newlines", true);

			/*var mediumRTFConfiguration = config.Empty().Clone()
                .Menubar("file edit insert view format table tools help")
                .ContentCss("/css/rtf_editor.css")
                .AddSetting("templates", templates)
                .AddSetting("force_br_newlines", true)
                .AddSetting(
                    "extended_valid_elements", // List of valid elements in the editor, this includes scritps (for js), iframe, and several others. What you send inside the [] are the allowed inner elements for that tag 
                    "script[language|type|src],iframe[src|alt|title|width|height|align|name|style],picture,source[srcset|media],a[id|href|target|onclick|class],span[*],div[*]")
                .AddPlugin(@"epi-link epi-image-editor epi-dnd-processor paste
                        epi-personalized-content preview searchreplace
                        autolink directionality visualblocks visualchars code
                        fullscreen image link media template codesample table charmap
                        pagebreak nonbreaking anchor insertdatetime advlist lists
                        wordcount imagetools help")
                .Toolbar("bold italic underline strikethrough charmap subscript superscript forecolor backcolor",
                    "numlist bullist outdent indent | alignleft aligncenter alignright alignjustify",
                    "epi-link | image epi-image-editor media | cut copy pastetext | removeformat code fullscreen",
                    "epi-personalized-content | formatselect | table | | codesample | template")
                .StyleFormats(textStyles)
                .BlockFormats("Paragraph=p;Heading 3=h3;Heading 4=h4;Heading 5=h5;Heading 6=h6");

            var emphasizedRTFConfiguration = config.Empty().Clone()
                .DisableMenubar()
                .ContentCss("/css/rtf_editor.css")
                .AddPlugin("epi-link link anchor wordcount lists fullscreen code charmap")
                .Toolbar("bold italic underline strikethrough charmap forecolor backcolor",
                    "numlist bullist | epi-link | cut copy pastetext | removeformat code fullscreen");
            */

			var limitedRTFConfiguration = config.Empty().Clone()
				.DisableMenubar()
				.ContentCss("/css/rtf_editor.css")
				.AddSetting("force_br_newlines", true)
				.AddPlugin("epi-link wordcount lists fullscreen code charmap")
				.Toolbar("bold italic underline subscript superscript charmap bullist | epi-link anchor | pastetext removeformat code fullscreen");

			// Profile to handle in-line scripts and styles.
			var jsConfigurationSettings = config.Default().Clone()
				.AddEpiserverSupport()
				.ContentCss("/css/rtf_editor.css")
				.AddSetting("valid_children", "+body[style]")
				.AddSetting("extended_valid_elements", "script[language|type|src]")
				.AddSetting("allow_script_urls", true)
				.AddSetting("force_br_newlines", false)
				.AddSetting("statusbar", false)
				.DisableMenubar()
				.Toolbar(
					"formatselect styleselect | bold italic underline strikethrough subscript " +
					"superscript | epi-personalized-content | removeformat | fullscreen preview | code")
				.BlockFormats("Paragraph=p;Heading 2=h2;Heading 3=h3;Heading 4=h4;Heading 5=h5;Heading 6=h6");

			//config.For<SOMEBLOCKTYPE>(m => m.RICHTEXTFIELD, RTFCONFIG);
		});
		return services;
	}
}