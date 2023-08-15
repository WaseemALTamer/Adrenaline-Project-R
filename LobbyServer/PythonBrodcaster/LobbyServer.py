import socket
import time
import threading

port = 3003

uav = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
uav.bind(("0.0.0.0", port))  # Bind to all available network interfaces
clients = []  # List to store client IP addresses



def DetectNewIncommers():
    while True:
        global uav
        data, addr = uav.recvfrom(1024)  # Receive data and client address
        client_ip = addr[0]
        
        if client_ip not in clients:
            clients.append(client_ip)
            print(f"New client connected: {client_ip}")
        print(f"Received message from {client_ip}: {data.decode()}")

threading.Thread(target=DetectNewIncommers).start()

while True:
    for client in clients:
        uav.sendto(f"Hello from server to {client}".encode(), (client, port))
    time.sleep(1)

