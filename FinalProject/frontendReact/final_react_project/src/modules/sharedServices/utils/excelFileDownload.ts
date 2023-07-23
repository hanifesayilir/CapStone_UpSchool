import api from "./AxiosInstance";


export const excelFileDownload = async (orderId : string) =>{
    try {

        const response = await api.post(`/Excels/WriteProducts`, {orderId: orderId}, { responseType: "arraybuffer"});
        const file = new Blob([response.data], { type: 'application/vnd.ms-excel' });
        const fileUrl = URL.createObjectURL(file);


        const link = document.createElement('a');
        link.href = fileUrl;
        link.download = 'products.xlsx';
        link.click();


        URL.revokeObjectURL(fileUrl);
        link.remove();

        return response;
    } catch (error) {
        //console.error('Error downloading Excel file:', error);
    }

}
