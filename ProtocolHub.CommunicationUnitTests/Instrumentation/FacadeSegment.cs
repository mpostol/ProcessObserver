//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.CommServer.ProtocolHub.Communication.BaseStation;
using CAS.Lib.CommonBus.ApplicationLayer;
using System;
using UAOOI.ProcessObserver.Configuration;
using UAOOI.ProcessObserver.RealTime.Processes;

namespace CAS.CommServer.ProtocolHub.CommunicationUnitTests.Instrumentation
{
  internal class FacadeSegment : WaitTimeList<Pipe.PipeInterface.PipeDataBlock>
  {
    internal class FacadeDataDescription : DataQueue.DataDescription
    {
      protected override TimeSpan TimeOut => throw new Exception("The method or operation is not implemented.");
      internal override TimeSpan TimeScann => new TimeSpan(0, 0, 0, 0, 1000);

      internal FacadeDataDescription(ComunicationNet.DataBlocksRow BlocksRow, ref int constraint)
        :
        base(BlocksRow, TimeSpan.MaxValue, null, null, ref constraint)
      { }
    }

    internal class FacadePipeInterface : Pipe.PipeInterface
    {
      /// <summary>
      /// Facade implementation of BaseStation.Pipe.PipeInterface.PipeDataBlock
      /// </summary>
      internal class FacadePipeDataBlock : Pipe.PipeInterface.PipeDataBlock
      {
        internal FacadePipeDataBlock(WaitTimeList<PipeDataBlock> waitTimeList, FacadeDataDescription dataDescription, FacadePipeInterface pipeInterface)
          : base(waitTimeList, dataDescription, pipeInterface)
        { }
      }

      protected internal override bool WriteData(object data, IBlockDescription dataAddress)
      {
        throw new NotImplementedException("The method or operation is not implemented.");
      }

      protected internal override bool ReadData(out object data, IBlockDescription dataAddress)
      {
        throw new Exception("The method or operation is not implemented.");
      }

      internal FacadePipeInterface(InterfaceParameters interfaceDSC, FacadePipe pipe, CAS.CommServer.ProtocolHub.Communication.Diagnostic.Segment segment)
        :
        base(interfaceDSC, pipe, null, segment, 10)
      { }
    }//FacadePipeInterface

    internal FacadeSegment() : base("NUnitTestFacadeSegment")
    {
    }
  }
}