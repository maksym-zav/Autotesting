using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System.Reflection;

namespace WebApps.CmnWebApp
{
    /// <summary>
    /// Decorates found elements to custom wrappers
    /// </summary>
    public class CustomPageObjectMemberDecorator : DefaultPageObjectMemberDecorator, IPageObjectMemberDecorator
    {
        private readonly IWebDriver driver;

        public CustomPageObjectMemberDecorator(IWebDriver driver)
        {
            this.driver = driver;
        }

        public override object Decorate(MemberInfo member, IElementLocator locator)
        {
            // Member should have any attribute
            if (member.GetCustomAttributes().Any())
            {
                // Member should be a property or field
                FieldInfo fieldInfo = member as FieldInfo;
                PropertyInfo propertyInfo = member as PropertyInfo;
                Type type = null;
                if (fieldInfo != null)
                {
                    type = fieldInfo.FieldType;
                }

                if (propertyInfo != null)
                {
                    type = propertyInfo.PropertyType;
                }

                if (type != null)
                {
                    // For List<Element> type
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        Type c = type.GetGenericArguments()[0];
                        if (typeof(Element).IsAssignableFrom(c))
                        {
                            IList<By> list = CreateLocatorList(member);
                            if (list.Count > 0)
                            {
                                List<IWebElement> elements = ((IList<IWebElement>)base.CreateObject(typeof(IList<IWebElement>), locator, list, ShouldCacheLookup(member))).ToList();
                                
                                var array = Array.CreateInstance(c, elements.Count);
                                for (int i = 0; i < elements.Count; i++)
                                {
                                    Element element = (Element)Activator.CreateInstance(c, driver, elements[i], list.First());
                                    array.SetValue(element, i);
                                }

                                // Create a list of the required type, passing the values to the constructor
                                Type concreteListType = typeof(List<>).MakeGenericType(c);
                                object elementsList = Activator.CreateInstance(concreteListType, new object[] { array });

                                return elementsList;
                            }
                        }
                    }
                    // For Element and all derived types
                    else
                    {
                        // Specified property type should be compatible with Element wrapper class
                        if (typeof(Element).IsAssignableFrom(type))
                        {
                            IList<By> list = CreateLocatorList(member);
                            if (list.Count > 0)
                            {
                                IWebElement element = (IWebElement)base.CreateObject(typeof(IWebElement), locator, list, ShouldCacheLookup(member));
                                return Activator.CreateInstance(type, driver, element, list.First());
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
