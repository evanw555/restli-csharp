using System;
using System.Collections.Generic;
using System.Linq;

using restlicsharpdata.restlidata;

// DO NOT EDIT - THIS FILE IS GENERATED BY restli-csharp
// Generated from com\linkedin\restli\common\PegasusSchema.pdsc

namespace com.linkedin.restli.common
{
  /// <summary>
/// A "marker" data schema for data that is itself a data schema (a "PDSC for PDSCs"). Because PDSC is not expressive enough to describe it's own format, this is only a marker, and has no fields. Despite having no fields, it is required that data marked with this schema be non-empty. Specifically, is required that data marked as using this schema fully conform to the PDSC format (https://github.com/linkedin/rest.li/wiki/DATA-Data-Schema-and-Templates#schema-definition).
/// </summary>
  public class PegasusSchema : RecordTemplate
  {

    public PegasusSchema(Dictionary<string, object> data)
    {
      object value;
    }

    public PegasusSchema(PegasusSchemaBuilder builder)
    {
    }

    public override Dictionary<string, object> Data()
    {
      Dictionary<string, object> dataMap = new Dictionary<string, object>();
      return dataMap;
    }

  }

  public class PegasusSchemaBuilder
  {

    public PegasusSchema Build()
    {
      return new PegasusSchema(this);
    }
  }
}
