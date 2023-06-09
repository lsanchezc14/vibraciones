from opcua import Client
from opcua import ua
from opcua.crypto import security_policies
import time

url="opc.tcp://127.0.0.1:4840"

client = Client(url)

client.connect()
print("Cliente conectado")


node=client.get_node("ns=2;i=2")
node.set_value(True)

