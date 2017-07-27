using System;
using System.Collections.Generic;
using System.Linq;

using restlicsharpdata.restlidata;

// DO NOT EDIT - THIS FILE IS GENERATED BY restli-csharp
// Generated from com\linkedin\restli\common\ErrorResponse.pdsc

namespace com.linkedin.restli.common
{
  
  public class ErrorDetails : RecordTemplate
  {

    public ErrorDetails(Dictionary<string, object> data)
    {
      object value;
    }

    public ErrorDetails(ErrorDetailsBuilder builder)
    {
    }

    public override Dictionary<string, object> Data()
    {
      Dictionary<string, object> dataMap = new Dictionary<string, object>();
      return dataMap;
    }

  }

  public class ErrorDetailsBuilder
  {

    public ErrorDetails Build()
    {
      return new ErrorDetails(this);
    }
  }
}