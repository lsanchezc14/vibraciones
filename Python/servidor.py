from opcua import Server
from opcua import ua
from random import randint
import datetime
import time

server = Server()
url = "opc.tcp://127.0.0.1:4840"
name = "OPCUA_SIMULATION_SERVER"
server.set_endpoint(url)

addspace= server.register_namespace(name)

print(addspace)

node = server.get_objects_node()

print('Nodo:')
print(node)
Param = node.add_object(addspace, "Parameters")

data = Param.add_variable(addspace,"Entero",0)
# data2 = Param.add_variable(addspace,"Mensaje",0)

data.set_writable()
# data2.set_writable()

server.start()
print("Inicia servidor {}".format(url))

# while True:
#     temp = True
#     # temp2 = "Password secreto"
#     TIME=datetime.datetime.now()
#     print(temp,TIME)
#     data.set_value(temp)
#     # data2.set_value(temp2)
#     time.sleep(5)
