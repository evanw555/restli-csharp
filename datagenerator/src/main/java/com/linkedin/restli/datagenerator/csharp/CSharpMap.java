/*
   Copyright (c) 2017 LinkedIn Corp.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

package com.linkedin.restli.datagenerator.csharp;

import com.linkedin.pegasus.generator.spec.ClassTemplateSpec;
import com.linkedin.pegasus.generator.spec.MapTemplateSpec;


/**
 * @author Evan Williams
 */
public class CSharpMap extends CSharpCollectionType {
  private final MapTemplateSpec _map;

  public CSharpMap(ClassTemplateSpec spec, CSharpDataTemplateGenerator dataTemplateGenerator) {
    super(spec);
    _map = (MapTemplateSpec) spec;
    ClassTemplateSpec valueSpec = _map.getCustomInfo() != null ? CSharpUtil.templateSpecFromCustomInfo(_map.getCustomInfo()) : _map.getValueClass();
    _elementType = dataTemplateGenerator.generate(valueSpec, _map.getValueDataClass());
  }

  @Override
  public String getName(NameModifier modifier) {
    switch (modifier) {
      case MUTABLE:
        return "Dictionary<string, " + getElementType().getName(NameModifier.MUTABLE) + ">";
      case BUILDER_NULLABLE:
      case BUILDER:
        return "Dictionary<string, " + getElementType().getName(NameModifier.BUILDER) + ">";
      case MUTABLE_SHALLOW:
        return "Dictionary<string, " + getElementType().getName(NameModifier.IMMUTABLE) + ">";
      case TYPED_DATAMAP:
        return "Dictionary<string, " + getElementType().getName(NameModifier.TYPED_DATAMAP) + ">";
      case GENERIC_DATAMAP:
        return "Dictionary<string, object>";
      default:
        return "IReadOnlyDictionary<string, " + getElementType().getName(NameModifier.IMMUTABLE) + ">";
    }
  }

  @Override
  public String coerceToDataMapExpression(SequentialIdentifierGenerator generator) {
    String identifier = generator.generateIdentifier();
    return String.format(".ToDictionary(%1$s => %1$s.Key, %1$s => (object)%1$s.Value%2$s)", identifier, getElementType().coerceToDataMapExpression(generator));
  }

  @Override
  public String coerceFromDataMapExpression(SequentialIdentifierGenerator generator, String previousIdentifier) {
    String identifier = generator.generateIdentifier();
    return String.format("(%4$s)(%1$s.ToDictionary(%2$s => %2$s.Key, %2$s => %3$s))",
        previousIdentifier,
        identifier,
        getElementType().coerceFromDataMapExpression(generator, identifier + ".Value"),
        getName(NameModifier.IMMUTABLE));
  }
}
