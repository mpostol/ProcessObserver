# Process Observer

## Preface 

Process-Observer is an archetype that allows creation consistent, homogeneous real-time representation of the underlying process. This representation is a kind of a process state and behavior replica, which exposes real-time process data to the network using standardized interfaces like OPC Classic, OPC Unified Architecture, OPC PubSub, AMQP, MQTT, etc. In other words, it supports Machine to Sensors Connectivity (M2S), i.e. it allows an open, uniform, secure and standards-based communication solution between sensors, actuators, controllers and the upper layer applications.

Process Observer archetype greatly reduces the whole complexity and decreases interdependence by decoupling application associations and underlying data communication routes. Additionally, it allows applying systematic design methodology and building information architecture independently of the underlying communication infrastructure.

A detailed description of this concept is covered by the article [Object Oriented Internet][OOI.ieeexplore]. By design this concept supports

- **Process Devices Interconnection** - synchronization of the process replica with the process state
- **Process Simulation** - simulation of the process behavior to recover unavailable data and ensure a safe testing environment
- **Resource Monitoring** - allowing to add information processing and networking infrastructure to be exposed consistently aggregated with the process replica
- **Server to Server Interactions** - supports a scenario in which Process Observer is the Client of a Server

### Process Devices Interconnection

To establish a process replica a data fetching mechanism is necessary. Data fetching is related to a variety of last-mile communication technologies, for example, RFID, WI-FI, VHF, Bluetooth, etc. Additionally, any modification of the replica data holders has to cause pushing the modifications to the process devices, e.g actuators, controllers, etc. One of the main objectives of using the Process Observer is to provide a uniform bridge between digital plant-floor devices and systems providing services at the process control and business management levels.

### Process Simulation

Process-Observer does not only play the role of a communication engine. Offering the possibility of creating simulators and publishing simulated data in the same way as the process data, the final process representation can be complemented by directly unavailable information obtained by processing current and historical values. To commence factory tests of any system, we need to build a testing environment. Using simulators instead of communication drivers, it is possible to seamlessly switch between production and test environments reducing the cost by order of magnitude.

### Resource Monitoring

In a production environment, monitoring and management of the recourses that make up the information processing and communication infrastructure are often of the same importance as access the real-time process data. Process-Observer allows for publishing data gathered from the active communication devices in the same way as the process data.

### Server to Server Interactions

In this scenario, one Server acts as a Client of another Server. In the presented architecture it is implemented using a dedicated OPC Classic or OPC UA DataProvider. Server to Server interactions allows the development of servers that exchange data with each other on a peer-to-peer or vertical hierarchy basis to offer redundancy, aggregation, concentration or layered data access management.

## Related work

The Process-Observer concept has been implemented as a generic communication engine used by the CAS CommServer Classic and Unified Architecture servers. This implementation is optimized for highly distributed applications. Engaging an intermediate component as a driver for plant-floor devices is a middleware archetype used worldwide in thousands of applications. But to provide a consistent sole representation of a distributed real-time process at the upper layer boundary - according to the model - the CommServer™ has to implement unique features optimizing utilization of the underlying communication infrastructure as follows:

- **Multi-Protocol** capability: many protocols can be implemented as DataProvider components and plugged-in and utilized simultaneously 
- **Multi-Medium** capability: any physical layer technology can be used to start building a communication stack
- **Multi-Channel** connectivity: numerous independent communication routes can be activated  simultaneously to gather raw process data
- communication paths and signals redundancy
- **Adaptive Retry Algorithm**: each protocol retries to acquire data after a communication error, but adapting the number of retries to current conditions allows to increase greatly the whole bandwidth; 
- **Adaptive Sampling Algorithm**: is responsible for adjusting the plant floor devices sampling rate according to the current process state 
- **Optimal Transfer Algorithm**: is responsible for minimizing the difference between the individual process data update rate as required by clients and the current sampling rate of process control units

To get detailed description explore the [CommServer Help documentation][CommSvr.Help].

## See also

- Mariusz Postol, [Object Oriented Internet](https://ieeexplore.ieee.org/abstract/document/7321562), [3rd International Conference on Innovative Network Systems and Applications](https://fedcsis.org/2015/inetsapp), 2015, [IEEE Xplore Digital Library](https://ieeexplore.ieee.org/abstract/document/7321562) [![DOI](https://img.shields.io/badge/DOI-10.15439%2F2015F160-blue)](https://fedcsis.org/proceedings/2015/pliks/160.pdf)
- [API Browser](https://mpostol.github.io/ProcessObserver/)
- [Process Observer - Main Technology Features](http://www.commsvr.com/Howitworks/Technologie.aspx)
- [CommServer\Theory of operation][CommSvr.Help]
- [OPC UA Makes Process Observer Archetype Possible](https://mpostol.wordpress.com/2014/05/13/opc-ua-makes-process-observer-archetype-possible/)

[CommSvr.Help]:https://commsvr-com.github.io/Documentation/CommServer
[OOI.ieeexplore]:https://ieeexplore.ieee.org/abstract/document/7321562

<?--
## Repositoy content

## Consider titles

- Machine to Sensors Connectivity (M2S)
- Machine to Process Connectivity (M2P)

Process Observer has been implemented as the component OPC UA and Classic servers members of CommServer products family.

ProtocolHub is part of ProcessObserver. 

-->


