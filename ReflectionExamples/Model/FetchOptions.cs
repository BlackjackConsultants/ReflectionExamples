using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.Serialization;

namespace ReflectionExamples2.Model {

    /// <summary>
    /// This class represents the Entity Fetch options.
    /// </summary>
    public static class FetchOptions {

        /// <summary>
        /// Do not fetch related entities.
        /// </summary>
        public const string None = "0";

        /// <summary>
        /// Fetch <see cref="LegalEntityDto.Organization"/>.
        /// </summary>
        public const string Domain = "1";

        /// <summary>
        /// Fetch <see cref="ContactBaseDto.ParentContact"/>.
        /// </summary>
        public const string Phone = "2";

        /// <summary>
        /// Fetch the <see cref="IndividualDto.Organization"/>.
        /// </summary>		
        public const string Address = "4";

        /// <summary>
        /// Fetch <see cref="FundInvestmentDealDto.Organization"/>.
        /// </summary>
        public const string State = "08";

        /// <summary>
        /// Fetch <see cref="FundInvestmentDealDto.Organization"/>.
        /// </summary>
        public const string Country = "10";

        /// <summary>
        /// Gets the <see cref="BigInteger"/> value representing the <see cref="FetchOptions"/>.
        /// </summary>
        /// <typeparam name="TDto">The DTO type that contains the given property path.</typeparam>
        /// <param name="propertyPath">The property path for the DTO type.</param>
        /// <returns>Returns the <see cref="BigInteger"/>.</returns>
        public static BigInteger AsBigInteger<TDto>(string propertyPath) {
            if (propertyPath == null)
                throw new ArgumentNullException("propertyPath");
            // process path
            return AsBigInteger(typeof(TDto), propertyPath);
        }

        /// <summary>
        /// Gets the <see cref="BigInteger"/> value representing the <see cref="FetchOptions"/>.
        /// </summary>
        /// <param name="type">The DTO type that contains the given property path.</param>
        /// <param name="propertyPath">The property path for the DTO type.</param>
        /// <returns>Returns the <see cref="BigInteger"/>.</returns>
        public static BigInteger AsBigInteger(Type type, string propertyPath) {
            if (propertyPath == null)
                throw new ArgumentNullException("propertyPath");
            // split path
            var path = propertyPath.Split('.');
            // processed dtos
            var processedProperties = new HashSet<string>();
            // process path
            return AsBigInteger(type, path, processedProperties);
        }

        private static BigInteger AsBigInteger(Type dtoType, string[] propertyPath, HashSet<string> processedProperties) {
            // check array is not empty
            if (propertyPath.Length > 0) {
                // process first item in path
                var propertyName = propertyPath[0].Trim();
                // check current dto type has not been processed
                if (processedProperties.Add(dtoType.FullName + ":" + propertyName)) {
                    // find property info (declared in T)
                    var propertyInfo = dtoType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    // check we found the property
                    if (propertyInfo != null) {
                        // process property info
                        return ProcessPropertyInfo(propertyInfo, propertyPath, processedProperties);
                    }
                    var result = BigInteger.Zero;
                    // check we need to look in Known Types associated to current type because nothing was found in the parents.
                    // check type has [KnownType] attribute (do not get attributes from parent classes)
                    var attributes = dtoType.GetCustomAttributes(typeof(KnownTypeAttribute), false);
                    // loop attributes
                    foreach (var attribute in attributes) {
                        // cast attribute
                        var knownType = (KnownTypeAttribute)attribute;
                        // check Type was defined and inherits from the dto type
                        if (knownType.Type != null && dtoType.IsAssignableFrom(knownType.Type))
                            result |= AsBigInteger(knownType.Type, propertyPath, processedProperties);
                        else if (knownType.Type == null) {
                            // find method info for known type method
                            var methodInfo = dtoType.GetMethod(knownType.MethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly);
                            if (methodInfo != null) {
                                // invoke method
                                var types = methodInfo.Invoke(null, new object[] { }) as IEnumerable<Type>;
                                if (types != null)
                                    // and inherits from the dto type
                                    types.Aggregate(result, (c, t) => t.IsAssignableFrom(dtoType) ? c | AsBigInteger(t, propertyPath, processedProperties) : c);
                            }
                        }
                    }
                    // check we need to process base class because nothing was found on the lower part of the hierarchy
                    if (dtoType.BaseType != null && typeof(object) != dtoType.BaseType && result == BigInteger.Zero) {
                        result = AsBigInteger(dtoType.BaseType, propertyPath, processedProperties);
                    }
                    return result;
                }
            }
            return BigInteger.Zero;
        }

        private static BigInteger ProcessPropertyInfo(PropertyInfo propertyInfo, string[] propertyPath, HashSet<string> processedProperties) {
            // find the [FetchOption] if any
            var attributes = propertyInfo.GetCustomAttributes(typeof(FetchOptionAttribute), true);
            if (attributes.Length == 1) {
                // check if we need to continue processing path
                if (propertyPath.Length > 1) {
                    // create new path (skip current property)
                    var path = propertyPath.Skip(1).ToArray();
                    // check property type is a IEnumerable<T>
                    if (propertyInfo.PropertyType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))) {
                        // use Fetch option value and continue processing path on generic parameter types
                        return propertyInfo.PropertyType.GetGenericArguments().Aggregate(AsBigInteger(((FetchOptionAttribute)attributes[0]).FetchOption), (c, t) => c | AsBigInteger(t, path, processedProperties));
                    }
                    // use Fetch option value and continue processing path
                    return AsBigInteger(((FetchOptionAttribute)attributes[0]).FetchOption) | AsBigInteger(propertyInfo.PropertyType, path, processedProperties);
                }
                // use Fetch option value
                return AsBigInteger(((FetchOptionAttribute)attributes[0]).FetchOption);
            }
            // check if we need to continue processing path on a property that has no fetch option
            // still we need to traverse the graph looking for other options.
            if (propertyPath.Length > 1) {
                // create new path (skip current property)
                var path = propertyPath.Skip(1).ToArray();
                // start processing on the new path
                return AsBigInteger(propertyInfo.PropertyType, path, processedProperties);
            }
            // return if there is nothing else to check along the path
            return BigInteger.Zero;
        }

        /// <summary>
        /// Gets the <see cref="BigInteger"/> value representing the <see cref="FetchOptions"/>.
        /// </summary>
        /// <param name="option">The <see cref="FetchOptions"/>.</param>
        /// <returns>Returns the <see cref="BigInteger"/>.</returns>
        public static BigInteger AsBigInteger(string option) {
            // check option
            if (!String.IsNullOrEmpty(option)) {
                // parse Hex number
                return BigInteger.Parse(option, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
            }
            return BigInteger.Zero;
        }

        /// <summary>
        /// Gets the <see cref="BigInteger"/> value as a Hex string.
        /// </summary>
        /// <param name="bigInteger">The <see cref="BigInteger"/> value.</param>
        /// <returns>The Hex representation.</returns>
        public static string AsString(BigInteger bigInteger) {
            return bigInteger.ToString("X", CultureInfo.InvariantCulture.NumberFormat);
        }
    }
}
