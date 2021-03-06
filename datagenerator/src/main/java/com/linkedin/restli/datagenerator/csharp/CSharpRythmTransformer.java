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

import com.linkedin.data.schema.DataSchema;
import com.linkedin.data.schema.RecordDataSchema;
import java.io.File;
import java.util.HashMap;
import java.util.Map;
import org.rythmengine.extension.Transformer;


/**
 * @author Evan Williams
 */
@Transformer("restli")
public class CSharpRythmTransformer {
  private static final Map<DataSchema.Type, String> typeNameMap = new HashMap<>();
  static
  {
    typeNameMap.put(DataSchema.Type.INT, "int");
    typeNameMap.put(DataSchema.Type.DOUBLE, "double");
    typeNameMap.put(DataSchema.Type.BOOLEAN, "bool");
    typeNameMap.put(DataSchema.Type.STRING, "string");
    typeNameMap.put(DataSchema.Type.FLOAT, "float");
    typeNameMap.put(DataSchema.Type.LONG, "long");
  }

  public static final String PATH_ROOT = File.separator + "com" + File.separator;

  public static String comment(String comment) {
    if (comment == null || comment.isEmpty()) {
      return "";
    } else if (comment.contains("\n")) {
      return "/*\n* " + comment.replaceAll("\n", "\n* ") + "\n*/";
    } else {
      return "// " + comment;
    }
  }

  public static String classComment(String comment) {
    if (comment == null || comment.isEmpty()) {
      return "";
    } else {
      return "/// <summary>\n/// " + comment.replaceAll("\n", "\n/// ") + "\n/// </summary>";
    }
  }

  public static String getNullableType(RecordDataSchema.Field field) {
    return typeNameMap.get(field.getType().getType()) + (field.getType().getType() != DataSchema.Type.STRING ? "?" : "");
  }

  public static String hasDefault(RecordDataSchema.Field field) {
    return Boolean.toString(field.getDefault() != null);
  }

  /**
   * Add n spaces to the start of each line, starting from the second line.
   * obj is Object instead of String because rythm considers the variable
   * created from assigning an template invocation result as an Object.
   *
   * @param obj source string
   * @param n number of spaces
   * @return indented string
   */
  public static String addIndent(Object obj, int n)
  {
    String s = obj.toString();

    String[] lines = s.split(GeneratorConstants.NEWLINE);
    String spaces = CSharpUtil.spaces(n);
    StringBuilder sb = new StringBuilder();
    for (int i = 0; i < lines.length; i++)
    {
      if (i > 0)
      {
        sb.append(spaces);
      }
      sb.append(lines[i]);
      if (i < lines.length - 1)
      {
        sb.append(GeneratorConstants.NEWLINE);
      }
    }
    return sb.toString();
  }

  public static String generatedFrom(CSharpType type) {
    final String location = type.getSpec().getLocation();
    if (location != null) {
      return "\n// Generated from " + location.substring(location.lastIndexOf(PATH_ROOT)+ 1);
    } else {
      return "";
    }
  }

  /**
   * Helper method to get string representation of object type
   * within a union's member list.
   */
  public static String getMemberListEntryType(Object obj) {
    if (obj instanceof Map.Entry) {
      Map.Entry entry = (Map.Entry) obj;
      return ((CSharpUnion.Member) entry.getValue()).type.getName();
    } else {
      return null;
    }
  }
}